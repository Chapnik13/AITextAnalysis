using Crawler.Analyzers.AnalysisResults;
using Crawler.Analyzers.Helpers;
using Crawler.DeJargonizer;
using Crawler.LexicalAnalyzer;
using Crawler.Models;
using System.Collections.Generic;
using System.Linq;

namespace Crawler.Analyzers
{
    public class SubtitleAnalyzer : IAnalyzer<SubtitleAnalysisResult>
    {
        private readonly IWordsAnalyzer wordsAnalyzer;
        private readonly IPunctuationAnalyzer punctuationAnalyzer;

        public SubtitleAnalyzer(IWordsAnalyzer wordsAnalyzer, IPunctuationAnalyzer punctuationAnalyzer)
        {
            this.wordsAnalyzer = wordsAnalyzer;
            this.punctuationAnalyzer = punctuationAnalyzer;
        }

        public SubtitleAnalysisResult Analyze(Article<List<Token>> article)
        {
            var subtitle = article.Subtitle;

            return new SubtitleAnalysisResult
            {
                AmountOfWords = wordsAnalyzer.CountWords(subtitle),
                AmountOfRareWords = wordsAnalyzer.CalculateDeJargonizer(subtitle).RareWords.Count(),
                AmountOfQuestionMarks = punctuationAnalyzer.CountCharacter('?', subtitle),
                AmountOfExclamationMarks = punctuationAnalyzer.CountCharacter('!', subtitle),
                AmountOfDashes = punctuationAnalyzer.CountCharacter('-', subtitle),
                AmountOfColons = punctuationAnalyzer.CountCharacter(':', subtitle),
                AmountOfQuotationMarks = punctuationAnalyzer.CountCharacter('"', subtitle)
            };
        }
    }
}
