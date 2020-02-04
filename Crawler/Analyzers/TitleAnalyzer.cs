using Crawler.Analyzers.AnalysisResults;
using Crawler.Analyzers.Helpers;
using Crawler.DeJargonizer;
using Crawler.LexicalAnalyzer;
using Crawler.Models;
using System.Collections.Generic;

namespace Crawler.Analyzers
{
    public class TitleAnalyzer : IAnalyzer<TitleAnalysisResult>
    {
        private readonly IDeJargonizer deJargonizer;

        public TitleAnalyzer(IDeJargonizer deJargonizer)
        {
            this.deJargonizer = deJargonizer;
        }

        public TitleAnalysisResult Analyze(Article<List<Token>> article)
        {
            var result = new TitleAnalysisResult();
            var title = article.Title;

            var wordsAnalyzer = new WordsAnalyzer(deJargonizer, title);

            result.AmountOfWords = wordsAnalyzer.CountWords();
            result.DeJargonizerResult = wordsAnalyzer.CalculateDeJargonizer();

            return result;
        }
    }
}
