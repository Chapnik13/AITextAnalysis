using Crawler.DeJargonizer;

namespace Crawler.Analyzers.AnalysisResults
{
    public class ContentAnalysisResult
    {
        public DeJargonizerResult DeJargonizerResult { get; set; }
        public double WordLengthStandartDeviation { get; set; }
        public double PercentageOfEmotionWords { get; set; }
        public int AmountOfQuestionWords { get; set; }
        public int AmountOfConjunctionWord { get; set; }
        public int AmountOfNumbersAsWords { get; set; }
        public int AmountOfNumbersAsDigits { get; set; }
        public float PercentageOfBoldWords { get; set; }
        public float AverageLengthOfParagraph { get; set; }
        public float AverageAmountOfSentencesInParagraph { get; set; }
        public float AverageAmountOfCommasAndPeriodsInParagraph { get; set; }
    }
}