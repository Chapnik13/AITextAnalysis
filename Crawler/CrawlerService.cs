using Crawler.Analyzers;
using Crawler.LexicalAnalyzer;
using Crawler.SiteScraper;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
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
        private readonly IWordsAnalyzer wordsAnalyzer;
        private readonly IParagraphAnalyzer paragraphAnalyzer;
        private readonly IPunctuationAnalyzer punctuationAnalyzer;

        public CrawlerService(IScraper scraper, ILexer lexer, IHostApplicationLifetime applicationLifetime, IWordsAnalyzer wordsAnalyzer,
            IParagraphAnalyzer paragraphAnalyzer, IPunctuationAnalyzer punctuationAnalyzer)
        {
            this.scraper = scraper;
            this.lexer = lexer;
            this.applicationLifetime = applicationLifetime;
            this.wordsAnalyzer = wordsAnalyzer;
            this.paragraphAnalyzer = paragraphAnalyzer;
            this.punctuationAnalyzer = punctuationAnalyzer;
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

            var wordsCount = allTokens.GetValuesByTokenTypes(eTokenType.StringValue, eTokenType.Number).Count();
            var averageAmountOfCommaAndPeriod = paragraphAnalyzer.CalculateAverageAmountOfCommaAndPeriod(paragraphsTokens);
            var numbersAsWords = wordsAnalyzer.CalculateNumbersAsWords(allTokens);
            var numbersAsDigits = wordsAnalyzer.CalculateNumbersAsDigits(allTokens);
            var questionWords = wordsAnalyzer.CalculateQuestionWords(allTokens);

            Console.WriteLine(text);
            Console.WriteLine($"average word length: {wordsAnalyzer.CalculateAverageLength(allTokens)}");
            Console.WriteLine($"word standard deviation: {wordsAnalyzer.CalculateWordsLengthStandardDeviation(allTokens)}");
            Console.WriteLine($"deJargonizer score: {deJargonizerResult.Score}");
            Console.WriteLine($"deJargonizer rare words percentage: {deJargonizerResult.RareWordsPercentage}");

            Console.WriteLine($"average paragraph length: {paragraphAnalyzer.CalculateAverageLength(paragraphsTokens)}");
            Console.WriteLine($"average amount of commas and periods in a paragraph: {Normalize(averageAmountOfCommaAndPeriod, wordsCount)}");
            Console.WriteLine($"average amount of sentences in a paragraph: {paragraphAnalyzer.CalculateAverageAmountOfSentences(paragraphsTokens)}");

			Console.WriteLine($"amount of numbers as words: {Normalize(numbersAsWords, wordsCount)}");
            Console.WriteLine($"amount of numbers as digits: {Normalize(numbersAsDigits, wordsCount)}");
            Console.WriteLine($"percentage of emotion words: {wordsAnalyzer.CalculatePercentageEmotionWords(allTokens)}");
            Console.WriteLine($"amount of question words: {Normalize(questionWords, wordsCount)}");

            Console.WriteLine($"average words count between punctuation: {punctuationAnalyzer.CalculateAverageWordsCountBetweenPunctuation(allTokens)}");
            Console.WriteLine($"max words count between punctuation: {punctuationAnalyzer.CalculateMaxWordsCountBetweenPunctuation(allTokens)}");
            Console.WriteLine($"standard deviation of words count between punctuation: {punctuationAnalyzer.CalculateWordsCountsBetweenPunctuationStandardDeviation(allTokens)}");
            Console.WriteLine($"ninth decile words count between punctuation: {punctuationAnalyzer.CalculateWordsCountDecile(9, allTokens)}");
            Console.WriteLine($"second decile words count between punctuation: {punctuationAnalyzer.CalculateWordsCountDecile(2, allTokens)}");

			applicationLifetime.StopApplication();
        }

        public double Normalize(float value, int currentScale)
        {
	        return value / currentScale * NORMALIZATION_COMMON_SCALE;
        }
    }
}