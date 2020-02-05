using Crawler.DeJargonizer;

namespace Crawler.Analyzers.AnalysisResults
{
    [AnalysisResult("Content")]
    public class ContentAnalysisResult
    {
        public DeJargonizerResult DeJargonizerResult { get; set; }
        [Result("Word length standart deviation: {0:0.00} words")]
        public double WordLengthStandartDeviation { get; set; }
        [Result("Percentage of emotions words: {0:0.00}%")]
        public double PercentageOfEmotionWords { get; set; }
        [Result("Amount Question words {0}")]
        public int AmountOfQuestionWords { get; set; }

        [Result("Amount of numbers written in letters {0}")]
        public int AmountOfNumbersAsWords { get; set; }
        [Result("Amount of numbers written in digits {0}")]
        public int AmountOfNumbersAsDigits { get; set; }
        [Result("Average length of paragraph {0} words")]
        public float AverageLengthOfParagraph { get; set; }
        [Result("Average amount of sentences in paragraph {0}")]
        public float AverageAmountOfSentencesInParagraph { get; set; }
        [Result("Average amount of commas and periods in paragraph {0}")]
        public float AverageAmountOfCommasAndPeriodsInParagraph { get; set; }
    }
}