using Crawler.Analyzers;
using Crawler.Analyzers.AnalysisResults;
using Crawler.LexicalAnalyzer;
using Crawler.Models;
using Crawler.SiteScraper;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Crawler
{
    public class CrawlerService : BackgroundService
    {
        private const int NORMALIZATION_COMMON_SCALE = 1000;

        private readonly IScraper scraper;
        private readonly ILexer lexer;
        private readonly IHostApplicationLifetime applicationLifetime;
        private readonly List<IAnalyzer<AnalysisResult>> analyzers;

        public CrawlerService(IScraper scraper, ILexer lexer, IHostApplicationLifetime applicationLifetime,
            IAnalyzer<TitleAnalysisResult> titleAnalyzer, IAnalyzer<SubtitleAnalysisResult> subtitleAnalyzer, IAnalyzer<ContentAnalysisResult> contentAnalyzer)
        {
            this.scraper = scraper;
            this.lexer = lexer;
            this.applicationLifetime = applicationLifetime;

            analyzers = new List<IAnalyzer<AnalysisResult>> { titleAnalyzer, subtitleAnalyzer, contentAnalyzer };
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

            foreach (var analyzer in analyzers)
            {
                DisplayResult(analyzer.Analyze(tokenizedArticle));
            }

            applicationLifetime.StopApplication();
        }

        private void DisplayResult(AnalysisResult analysisResult)
        {
            var analysisResultAttr = analysisResult.GetType().GetCustomAttribute<AnalysisResultAttribute>();
            if (analysisResultAttr != null)
            {
                Console.WriteLine($"===================={analysisResultAttr.ArticlePart}====================");
            }

            foreach (var prop in analysisResult.GetType().GetProperties())
            {
                var resultAttr = prop.GetCustomAttribute<ResultAttribute>();
                if (resultAttr != null)
                {
                    var value = double.Parse(prop.GetValue(analysisResult).ToString());

                    if (prop.GetCustomAttribute<NormalizeAttribute>() != null)
                    {
                        value = Normalize(value, analysisResult.AmountOfWords);
                        Console.Write("(Normalized) ");
                    }

                    Console.WriteLine($"{resultAttr.Description}: {value:0.##} {resultAttr.Unit}");
                }
            }
        }

        private double Normalize(double value, int currentScale)
        {
            return value / currentScale * NORMALIZATION_COMMON_SCALE;
        }
    }
}