using Crawler.DeJargonizer;

namespace Crawler.Analyzers.AnalysisResults
{
    public class SubtitleAnalysisResult
    {
        public int AmountOfWords { get; set; }
        public DeJargonizerResult DeJargonizerResult { get; set; }
        public int AmountOfPunctuation { get; set; }
        public float PercentageOfBoldWords { get; set; }
        public int AmountOfConjunctionWord { get; set; }
    }
}