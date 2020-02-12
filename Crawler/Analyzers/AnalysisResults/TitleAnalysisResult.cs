namespace Crawler.Analyzers.AnalysisResults
{
    [AnalysisResult("Title")]
    public class TitleAnalysisResult : AnalysisResult
    {
        [Result("Amount of rare words")]
        public int AmountOfRareWords { get; set; }
        [Result("Amount of punctuation characters")]
        public int AmountOfPunctuation { get; set; }
    }
}