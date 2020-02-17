using Crawler.Analyzers.Attributes;

namespace Crawler.Analyzers.Title
{
    [AnalysisResult("Title")]
    public class TitleAnalysisResult : AnalysisResult
    {
        [Normalize]
        [Result("Amount of rare words")]
        public int AmountOfRareWords { get; set; }

        [Normalize]
        [Result("Amount of '?'")]
        public int AmountOfQuestionMarks { get; set; }

        [Normalize]
        [Result("Amount of '!'")]
        public int AmountOfExclamationMarks { get; set; }

        [Normalize]
        [Result("Amount of ':'")]
        public int AmountOfColons { get; set; }

        [Normalize]
        [Result("Amount of '-'")]
        public int AmountOfDashes { get; set; }

        [Normalize]
        [Result("Amount of '\"'")]
        public int AmountOfQuotationMarks { get; set; }
    }
}