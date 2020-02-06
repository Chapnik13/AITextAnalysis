using Crawler.Analyzers.AnalysisResults;
using Crawler.Analyzers.Helpers;
using Crawler.DeJargonizer;
using Crawler.ExtensionMethods;
using Crawler.LexicalAnalyzer;
using Crawler.Models;
using System.Collections.Generic;
using System.Linq;

namespace Crawler.Analyzers
{
    public class ContentAnalyzer : IAnalyzer<ContentAnalysisResult>
    {
        private readonly IDeJargonizer deJargonizer;

        public ContentAnalyzer(IDeJargonizer deJargonizer)
        {
            this.deJargonizer = deJargonizer;
        }

        public ContentAnalysisResult Analyze(Article<List<Token>> article)
        {
            var content = article.Content;
            var wordsAnalyzer = new WordsAnalyzer(deJargonizer, content.SelectMany(t => t).ToList());
            var deJargonizerResult = wordsAnalyzer.CalculateDeJargonizer();

            return new ContentAnalysisResult
            {
                AmountOfNumbersAsWords = wordsAnalyzer.CalculateNumbersAsWords(),
                AmountOfNumbersAsDigits = wordsAnalyzer.CalculateNumbersAsDigits(),
                AmountOfQuestionWords = wordsAnalyzer.CalculateQuestionWords(),
                PercentageOfEmotionWords = wordsAnalyzer.CalculatePercentageEmotionWords() * 100,
                WordLengthStandartDeviation = wordsAnalyzer.CalculateStandardDeviation(),
                DeJargonizerScore = deJargonizerResult.Score,
                AmountOfRareWords = deJargonizerResult.RareWords.Count(),
                AverageLengthOfParagraph = CalculateAverageParagraphLength(content),
                AverageAmountOfSentencesInParagraph = CalculateAverageAmountOfSentencesInParagraph(content),
                AverageAmountOfCommasAndPeriodsInParagraph = CalculateAverageAmountOfCommasAndPeriodsInParagraph(content)
            };
        }

        private float CalculateAverageParagraphLength(List<List<Token>> paragraphs)
        {
            return paragraphs.CalculateAverageOfTokenGroups(t => t.TokenType != eTokenType.Punctuation);
        }

        private float CalculateAverageAmountOfCommasAndPeriodsInParagraph(List<List<Token>> paragraphs)
        {
            return paragraphs.CalculateAverageOfTokenGroups(t => t.Value == "." || t.Value == ",");
        }

        private float CalculateAverageAmountOfSentencesInParagraph(List<List<Token>> paragraphs)
        {
            return paragraphs.CalculateAverageOfTokenGroups(t => t.Value == ".");
        }
    }
}
