using Crawler.Analyzers;
using Crawler.LexicalAnalyzer;
using Crawler.Models;
using Crawler.SiteScraper;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
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
                Paragraphs = article.Paragraphs.Select(paragraph => lexer.GetTokens(paragraph).ToList()).ToList()
            };

            var allTokens = tokenizedArticle.Paragraphs.SelectMany(t => t).ToList();

            var deJargonizerResult = wordsAnalyzer.CalculateDeJargonizer(allTokens);

            Console.WriteLine("================ Text ================");
            Console.WriteLine($"average word length: {wordsAnalyzer.CalculateAverageLength(allTokens)}");
            Console.WriteLine($"word standard deviation: {wordsAnalyzer.CalculateStandardDeviation(allTokens)}");
            Console.WriteLine($"deJargonizer score: {deJargonizerResult.Score}");
            Console.WriteLine($"deJargonizer rare words percentage: {deJargonizerResult.RareWordsPercentage}");

            Console.WriteLine($"average paragraph length: {paragraphAnalyzer.CalculateAverageLength(tokenizedArticle.Paragraphs)}");
            Console.WriteLine($"average amount of commans and periods in a paragraph: {paragraphAnalyzer.CalculateAverageAmountOfCommaAndPeriod(tokenizedArticle.Paragraphs)}");
            Console.WriteLine($"average amount of sentences in a paragraph: {paragraphAnalyzer.CalculateAverageAmountOfSentences(tokenizedArticle.Paragraphs)}");
            Console.WriteLine("================ Text ================");
            Console.WriteLine("================ Title ================");
            Console.WriteLine($"Amount of words: {tokenizedArticle.Title.Count(t => t.TokenType == eTokenType.StringValue || t.TokenType == eTokenType.Number)}");
            Console.WriteLine("================ Title ================");
            Console.WriteLine("================ Subtitle ================");
            Console.WriteLine($"Amount of words: {tokenizedArticle.Subtitle.Count(t => t.TokenType == eTokenType.StringValue || t.TokenType == eTokenType.Number)}");
            Console.WriteLine("================ Subtitle ================");
            applicationLifetime.StopApplication();
        }
    }
}