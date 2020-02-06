namespace Crawler.Analyzers.AnalysisResults
{
    [AnalysisResult("Subtitle")]
    public class SubtitleAnalysisResult
    {
        [Result("Amount of words: {0}")]
        public int AmountOfWords { get; set; }
        [Result("Amount of rare words: {0}")]
        public int AmountOfRareWords { get; set; }
        [Result("Amount of punctuation characters: {0}")]
        public int AmountOfPunctuation { get; set; }
    }
}