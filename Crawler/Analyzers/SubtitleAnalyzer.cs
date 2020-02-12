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

        public SubtitleAnalyzer(IWordsAnalyzer wordsAnalyzer)
        {
            this.wordsAnalyzer = wordsAnalyzer;
        }

        public SubtitleAnalysisResult Analyze(Article<List<Token>> article)
        {
            var subtitle = article.Subtitle;

            return new SubtitleAnalysisResult
            {
                AmountOfWords = wordsAnalyzer.CountWords(subtitle),
                AmountOfRareWords = wordsAnalyzer.CalculateDeJargonizer(subtitle).RareWords.Count()
            };
        }
    }
}
