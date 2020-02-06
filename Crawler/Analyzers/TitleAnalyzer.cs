using Crawler.Analyzers.AnalysisResults;
using Crawler.Analyzers.Helpers;
using Crawler.DeJargonizer;
using Crawler.LexicalAnalyzer;
using Crawler.Models;
using System.Collections.Generic;
using System.Linq;

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
            var title = article.Title;
            var wordsAnalyzer = new WordsAnalyzer(deJargonizer, title);

            return new TitleAnalysisResult
            {
                AmountOfWords = wordsAnalyzer.CountWords(),
                AmountOfRareWords = wordsAnalyzer.CalculateDeJargonizer().RareWords.Count()
            };
        }
    }
}
