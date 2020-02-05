using Crawler.DeJargonizer;

namespace Crawler.Analyzers.AnalysisResults
{
    [AnalysisResult("Subtitle")]
    public class SubtitleAnalysisResult
    {
        [Result("Amount of words: {0}")]
        public int AmountOfWords { get; set; }
        public DeJargonizerResult DeJargonizerResult { get; set; }
        [Result("Amount of punctuation characters: {0}")]
        public int AmountOfPunctuation { get; set; }
    }
}