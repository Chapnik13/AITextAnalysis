using Crawler.Analyzers;
using Crawler.LexicalAnalyzer;
using Crawler.SiteScraper;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Crawler
{
    public class CrawlerService : BackgroundService
    {
        private readonly IScraper scraper;
        private readonly ILexer lexer;
        private readonly IHostApplicationLifetime applicationLifetime;
        private readonly IWordsAnalyzer wordsAnalyzer;
        private readonly IParagraphAnalyzer paragraphAnalyzer;

        public CrawlerService(IScraper scraper, ILexer lexer, IHostApplicationLifetime applicationLifetime, IWordsAnalyzer wordsAnalyzer,
            IParagraphAnalyzer paragraphAnalyzer)
        {
            this.scraper = scraper;
            this.lexer = lexer;
            this.applicationLifetime = applicationLifetime;
            this.wordsAnalyzer = wordsAnalyzer;
            this.paragraphAnalyzer = paragraphAnalyzer;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.Write("Enter url ");
            var url = Console.ReadLine();

            var paragraphsStrings = await scraper.ScrapAsync(url, cancellationToken)
                .ConfigureAwait(false);

            if (!paragraphsStrings.Any())
            {
                Console.WriteLine($"No text was found at {url}");
            }

            var paragraphsTokens = paragraphsStrings.Select(paragraph => lexer.GetTokens(paragraph)).ToList();
            var allTokens = paragraphsTokens.SelectMany(t => t).ToList();
            var text = paragraphsStrings.Aggregate((s1, s2) => $"{s1}{Environment.NewLine}{s2}");

            var deJargonizerResult = wordsAnalyzer.CalculateDeJargonizer(allTokens);

            Console.WriteLine(text);
            Console.WriteLine($"average word length: {wordsAnalyzer.CalculateAverageLength(allTokens)}");
            Console.WriteLine($"word standard deviation: {wordsAnalyzer.CalculateStandardDeviation(allTokens)}");
            Console.WriteLine($"deJargonizer score: {deJargonizerResult.Score}");
            Console.WriteLine($"deJargonizer rare words percentage: {deJargonizerResult.RareWordsPercentage}");

            Console.WriteLine($"average paragraph length: {paragraphAnalyzer.CalculateAverageLength(paragraphsTokens)}");
            Console.WriteLine($"average amount of commans and periods in a paragraph: {paragraphAnalyzer.CalculateAverageAmountOfCommaAndPeriod(paragraphsTokens)}");
            Console.WriteLine($"average amount of sentences in a paragraph: {paragraphAnalyzer.CalculateAverageAmountOfSentences(paragraphsTokens)}");

            applicationLifetime.StopApplication();
        }
    }
}