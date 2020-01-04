using Crawler.Analyzers;
using Crawler.LexicalAnalyzer;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crawler.SiteScraper;

namespace Crawler
{
	public class CrawlerService : BackgroundService
    {
        private readonly IScraper scraper;
        private readonly ILexer lexer;
        private readonly IHostApplicationLifetime applicationLifetime;
        private readonly IWordsAnalyzer wordsAnalyzer;

        public CrawlerService(IScraper scraper, ILexer lexer, IHostApplicationLifetime applicationLifetime, IWordsAnalyzer wordsAnalyzer)
        {
            this.scraper = scraper;
            this.lexer = lexer;
            this.applicationLifetime = applicationLifetime;
            this.wordsAnalyzer = wordsAnalyzer;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.Write("Enter url ");
            var url = Console.ReadLine();

            var stringParagraphs = await scraper.ScrapAsync(url, cancellationToken)
                .ConfigureAwait(false);

            if (!stringParagraphs.Any())
            {
                Console.WriteLine($"No text was found at {url}");
            }

            var tokenParagraphs = stringParagraphs.Select(paragraph => lexer.GetTokens(paragraph)).ToList();
            var allTokens = tokenParagraphs.SelectMany(t => t).ToList();

            var averageLength = wordsAnalyzer.CalculateAverageLength(allTokens);
            var standardDeviation = wordsAnalyzer.CalculateStandardDeviation(allTokens);
            var deJargonizerResult = wordsAnalyzer.CalculateDeJargonizer(allTokens);

            Console.WriteLine(stringParagraphs.Aggregate((s1, s2) => $"{s1}{Environment.NewLine}{s2}"));
            Console.WriteLine($"average length: {averageLength}");
            Console.WriteLine($"standard deviation: {standardDeviation}");
            Console.WriteLine($"deJargonizer score: {deJargonizerResult.Score}");
            Console.WriteLine($"deJargonizer rare words percentage: {deJargonizerResult.RareWordsPercentage}");

            applicationLifetime.StopApplication();
        }
    }
}