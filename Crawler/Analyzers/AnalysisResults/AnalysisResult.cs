namespace Crawler.Analyzers.AnalysisResults
{
    public abstract class AnalysisResult
    {
        [Result("Amount of words")]
        public int AmountOfWords { get; set; }
    }
}
