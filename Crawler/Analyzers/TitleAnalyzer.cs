using Crawler.Analyzers.AnalysisResults;
using Crawler.Analyzers.Helpers;
using Crawler.LexicalAnalyzer;
using Crawler.Models;
using System.Collections.Generic;
using System.Linq;

namespace Crawler.Analyzers
{
    public class TitleAnalyzer : IAnalyzer<TitleAnalysisResult>
    {
        private readonly IWordsAnalyzer wordsAnalyzer;
        private readonly IPunctuationAnalyzer punctuationAnalyzer;

        public TitleAnalyzer(IWordsAnalyzer wordsAnalyzer, IPunctuationAnalyzer punctuationAnalyzer)
        {
            this.wordsAnalyzer = wordsAnalyzer;
            this.punctuationAnalyzer = punctuationAnalyzer;
        }

        public TitleAnalysisResult Analyze(Article<List<Token>> article)
        {
            var title = article.Title;

            return new TitleAnalysisResult
            {
                AmountOfWords = wordsAnalyzer.CountWords(title),
                AmountOfRareWords = wordsAnalyzer.CalculateDeJargonizer(title).RareWords.Count(),
                AmountOfQuestionMarks = punctuationAnalyzer.CountCharacter('?', title),
                AmountOfExclamationMarks = punctuationAnalyzer.CountCharacter('!', title),
                AmountOfDashes = punctuationAnalyzer.CountCharacter('-',title),
                AmountOfColons = punctuationAnalyzer.CountCharacter(':', title),
                AmountOfQuotationMarks = punctuationAnalyzer.CountCharacter('"', title)
            };
        }
    }
}
