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
        private readonly IDeJargonizer deJargonizer;

        public SubtitleAnalyzer(IDeJargonizer deJargonizer)
        {
            this.deJargonizer = deJargonizer;
        }

        public SubtitleAnalysisResult Analyze(Article<List<Token>> article)
        {

            var subtitle = article.Subtitle;
            var wordsAnalyzer = new WordsAnalyzer(deJargonizer, subtitle);

            return new SubtitleAnalysisResult
            {
                AmountOfWords = wordsAnalyzer.CountWords(),
                AmountOfRareWords = wordsAnalyzer.CalculateDeJargonizer().RareWords.Count()
            };
        }
    }
}
