using Crawler.Analyzers.AnalysisResults;
using Crawler.Analyzers.Helpers;
using Crawler.DeJargonizer;
using Crawler.LexicalAnalyzer;
using Crawler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            var result = new ContentAnalysisResult();
            var content = article.Content;

            var wordsAnalyzer = new WordsAnalyzer(deJargonizer, content.SelectMany(t => t).ToList());
            var paragraphsAnalyzer = new ParagraphAnalyzer(content);

            result.AmountOfNumbersAsWords = wordsAnalyzer.CalculateNumbersAsWords();
            result.AmountOfNumbersAsDigits = wordsAnalyzer.CalculateNumbersAsDigits();
            result.AmountOfQuestionWords = wordsAnalyzer.CalculateQuestionWords();
            result.PercentageOfEmotionWords = wordsAnalyzer.CalculatePercentageEmotionWords();
            result.WordLengthStandartDeviation = wordsAnalyzer.CalculateStandardDeviation();
            result.DeJargonizerResult = wordsAnalyzer.CalculateDeJargonizer();

            return result;
        }
    }
}
