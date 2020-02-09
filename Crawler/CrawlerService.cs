using Crawler.Analyzers;
using Crawler.Analyzers.AnalysisResults;
using Crawler.DeJargonizer;
using Crawler.LexicalAnalyzer;
using Crawler.Models;
using Crawler.SiteScraper;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Crawler.ExtensionMethods;

namespace Crawler
{
    public class CrawlerService : BackgroundService
    {
	    private const int NORMALIZATION_COMMON_SCALE = 1000;

        private readonly IScraper scraper;
        private readonly ILexer lexer;
        private readonly IHostApplicationLifetime applicationLifetime;
        private readonly IDeJargonizer deJargonizer;

        public CrawlerService(IScraper scraper, ILexer lexer, IHostApplicationLifetime applicationLifetime, IDeJargonizer deJargonizer)
        {
            this.scraper = scraper;
            this.lexer = lexer;
            this.applicationLifetime = applicationLifetime;
            this.deJargonizer = deJargonizer;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.Write("Enter url ");
            var url = Console.ReadLine();

            var article = await scraper.ScrapAsync(url, cancellationToken)
                .ConfigureAwait(false);

            if (article is null)
            {
                Console.WriteLine($"No text was found at {url}");
                return;
            }

            var tokenizedArticle = new Article<List<Token>>
            {
                Title = lexer.GetTokens(article.Title).ToList(),
                Subtitle = lexer.GetTokens(article.Subtitle).ToList(),
                Content = article.Content.Select(paragraph => lexer.GetTokens(paragraph).ToList()).ToList()
            };

            foreach (var result in AnalyzeArticle(tokenizedArticle))
            {
                DisplayResult(result);
            }

            applicationLifetime.StopApplication();
        }

        private IEnumerable AnalyzeArticle(Article<List<Token>> article)
        {
            var analyzers = Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IAnalyzer<>)));

            foreach (var analyzerType in analyzers)
            {
                var analyzer = Activator.CreateInstance(analyzerType, new object[] { deJargonizer }) as IAnalyzer<object>;

                yield return analyzer.Analyze(article);
            }

        }

        private void DisplayResult(object obj)
        {
            var analysisResultAttr = obj.GetType().GetCustomAttribute<AnalysisResultAttribute>();
            if (analysisResultAttr != null)
            {
                Console.WriteLine($"===================={analysisResultAttr.ArticlePart}====================");
            }

            foreach (var prop in obj.GetType().GetProperties())
            {
                var resultAttr = prop.GetCustomAttribute<ResultAttribute>();
                if (resultAttr != null)
                {
                    Console.WriteLine(resultAttr.Format,prop.GetValue(obj));
                }
            }
        }


        public double Normalize(float value, int currentScale)
        {
	        return value / currentScale * NORMALIZATION_COMMON_SCALE;
        }
    }
}