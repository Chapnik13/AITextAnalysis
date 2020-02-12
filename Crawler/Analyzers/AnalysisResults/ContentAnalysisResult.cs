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

        [Normalize]
        [Result("Amount Question words")]
        public int AmountOfQuestionWords { get; set; }

        [Normalize]
        [Result("Amount of numbers written in letters")]
        public int AmountOfNumbersAsWords { get; set; }

        [Normalize]
        [Result("Amount of numbers written in digits")]
        public int AmountOfNumbersAsDigits { get; set; }

        [Result("Average length of paragraph", "words")]
        public float AverageLengthOfParagraph { get; set; }

        [Result("Average amount of sentences in paragraph")]
        public float AverageAmountOfSentencesInParagraph { get; set; }

        [Result("Average amount of commas and periods in paragraph")]
        public float AverageAmountOfCommasAndPeriodsInParagraph { get; set; }

        [Result("Average amount of words between punctuation")]
        public double AverageAmountOfWordsBetweenPunctuation { get; set; }

        [Result("Maximum words between punctuation")]
        public int MaxAmountOfWordsBetweenPunctuation { get; set; }

        [Result("Standard deviation of words between punctuation")]
        public double AmountOfWordsBetweenPunctuationStandardDeviation { get; set; }

        [Normalize]
        [Result("Second decile of amount of words between punctuation")]
        public float SecondDecileBetweenPunctuation { get; set; }

        [Normalize]
        [Result("Ninth decile of amount of words between punctuation")]
        public float NinthDecileBetweenPunctuation { get; set; }

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