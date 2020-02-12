using Crawler.Analyzers.AnalysisResults;
using Crawler.Analyzers.Helpers;
using Crawler.LexicalAnalyzer;
using Crawler.Models;
using System.Collections.Generic;
using System.Linq;

namespace Crawler.Analyzers
{
    public class ContentAnalyzer : IAnalyzer<ContentAnalysisResult>
    {
        private readonly IWordsAnalyzer wordsAnalyzer;
        private readonly IPunctuationAnalyzer punctuationAnalyzer;
        private readonly IParagraphAnalyzer paragraphAnalyzer;

        public ContentAnalyzer(IWordsAnalyzer wordsAnalyzer, IPunctuationAnalyzer punctuationAnalyzer, IParagraphAnalyzer paragraphAnalyzer)
        {
            this.wordsAnalyzer = wordsAnalyzer;
            this.punctuationAnalyzer = punctuationAnalyzer;
            this.paragraphAnalyzer = paragraphAnalyzer;
        }

        public ContentAnalysisResult Analyze(Article<List<Token>> article)
        {
            var contentAsParagraphs = article.Content;
            var contentAsText = contentAsParagraphs.SelectMany(t => t).ToList();
            var deJargonizerResult = wordsAnalyzer.CalculateDeJargonizer(contentAsText);

            return new ContentAnalysisResult
            {
                AmountOfWords = wordsAnalyzer.CountWords(contentAsText),
                AmountOfNumbersAsWords = wordsAnalyzer.CalculateNumbersAsWords(contentAsText),
                AmountOfNumbersAsDigits = wordsAnalyzer.CalculateNumbersAsDigits(contentAsText),
                AmountOfQuestionWords = wordsAnalyzer.CalculateQuestionWords(contentAsText),
                PercentageOfEmotionWords = wordsAnalyzer.CalculatePercentageEmotionWords(contentAsText) * 100,
                WordLengthStandardDeviation = wordsAnalyzer.CalculateWordsLengthStandardDeviation(contentAsText),
                DeJargonizerScore = deJargonizerResult.Score,
                AmountOfRareWords = deJargonizerResult.RareWords.Count(),
                AverageLengthOfParagraph = paragraphAnalyzer.CalculateAverageLength(contentAsParagraphs),
                AverageAmountOfSentencesInParagraph = paragraphAnalyzer.CalculateAverageAmountOfSentences(contentAsParagraphs),
                AverageAmountOfCommasAndPeriodsInParagraph = paragraphAnalyzer.CalculateAverageAmountOfCommasAndPeriods(contentAsParagraphs)
            };
        }

    }
}
