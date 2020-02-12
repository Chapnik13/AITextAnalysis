namespace Crawler.Analyzers.AnalysisResults
{
    [AnalysisResult("Content")]
    public class ContentAnalysisResult : AnalysisResult
    {
        [Normalize]
        [Result("Amount of rare words")]
        public int AmountOfRareWords { get; set; }
        [Result("DeJargonizer score")]
        public int DeJargonizerScore { get; set; }
        [Result("Word length standard deviation", "words")]
        public double WordLengthStandardDeviation { get; set; }
        [Result("Percentage of emotions words", "%")]
        public double PercentageOfEmotionWords { get; set; }
        [Result("Amount Question words")]
        public int AmountOfQuestionWords { get; set; }

        [Result("Amount of numbers written in letters")]
        public int AmountOfNumbersAsWords { get; set; }
        [Result("Amount of numbers written in digits")]
        public int AmountOfNumbersAsDigits { get; set; }
        [Result("Average length of paragraph", "words")]
        public float AverageLengthOfParagraph { get; set; }
        [Result("Average amount of sentences in paragraph")]
        public float AverageAmountOfSentencesInParagraph { get; set; }
        [Result("Average amount of commas and periods in paragraph")]
        public float AverageAmountOfCommasAndPeriodsInParagraph { get; set; }
    }
}