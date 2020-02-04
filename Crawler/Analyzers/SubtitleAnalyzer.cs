using Crawler.Analyzers.AnalysisResults;
using Crawler.Analyzers.Helpers;
using Crawler.DeJargonizer;
using Crawler.LexicalAnalyzer;
using Crawler.Models;
using System.Collections.Generic;

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
            var result = new SubtitleAnalysisResult();
            var subtitle = article.Subtitle;

            var wordsAnalyzer = new WordsAnalyzer(deJargonizer, subtitle);

            result.AmountOfWords = wordsAnalyzer.CountWords();
            result.DeJargonizerResult = wordsAnalyzer.CalculateDeJargonizer();

            return result;
        }
    }
}
