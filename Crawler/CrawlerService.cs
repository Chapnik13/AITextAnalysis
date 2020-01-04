﻿using Crawler.Analyzers;
using Crawler.LexicalAnalyzer;
using Microsoft.Extensions.Hosting;
using System;
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

            var text = await scraper.ScrapAsync(url, cancellationToken)
                .ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine($"No text was found at {url}");
            }

            var tokens = lexer.GetTokens(text);

            var averageLength = wordsAnalyzer.CalculateAverageLength(tokens);
            var standardDeviation = wordsAnalyzer.CalculateStandardDeviation(tokens);
            var deJargonizerResult = wordsAnalyzer.CalculateDeJargonizer(tokens);

            Console.WriteLine(text);
            Console.WriteLine($"average length: {averageLength}");
            Console.WriteLine($"standard deviation: {standardDeviation}");
            Console.WriteLine($"deJargonizer score: {deJargonizerResult.Score}");
            Console.WriteLine($"deJargonizer rare words percentage: {deJargonizerResult.RareWordsPercentage}");

            applicationLifetime.StopApplication();
        }
    }
}