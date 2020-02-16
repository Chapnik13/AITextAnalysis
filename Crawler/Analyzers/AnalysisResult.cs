using Crawler.Analyzers.Attributes;

namespace Crawler.Analyzers
{
    public abstract class AnalysisResult
    {
        [Result("Amount of words")]
        public int AmountOfWords { get; set; }
    }
}
