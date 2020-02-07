using Crawler.DeJargonizer;
using Crawler.Exceptions;
using Crawler.ExtensionMethods;
using Crawler.LexicalAnalyzer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Crawler.Analyzers.Helpers
{
    public class WordsAnalyzer
    {
        private const string EMOTIOMS_FILE = "Emotion.csv";
        private const string NUMBERS_FILE = "Numbers.csv";
        private const string QUESTIONS_FILE = "Questions.csv";

        private readonly IDeJargonizer deJargonizer;
        private readonly IEnumerable<Token> tokens;

        public WordsAnalyzer(IDeJargonizer deJargonizer, IEnumerable<Token> tokens)
        {
            this.deJargonizer = deJargonizer;
            this.tokens = tokens;
        }

        public int CountWords()
        {
            return tokens.Count(t => t.TokenType == eTokenType.StringValue || t.TokenType == eTokenType.Number);
        }

        public float CalculateAverageLength()
        {
            var words = tokens.GetValuesByTokenType(eTokenType.StringValue);

            return words.Any() ? (float)words.Average(w => w.Length) : 0;
        }

        public double CalculateStandardDeviation()
        {
            var words = tokens.GetValuesByTokenType(eTokenType.StringValue);

            if (words.Count() < 2) throw new StandardDeviationInvalidArgumentsAmountException();

            var average = CalculateAverageLength();
            var sumOfSquaresOfDifferences = words.Select(val => (val.Length - average) * (val.Length - average)).Sum();

            return Math.Sqrt(sumOfSquaresOfDifferences / (words.Count() - 1));
        }

        public DeJargonizerResult CalculateDeJargonizer()
        {
            var words = tokens.GetValuesByTokenType(eTokenType.StringValue);

            return deJargonizer.Analyze(words);
        }

        public int CalculateNumbersAsDigits()
        {
            var words = tokens.GetValuesByTokenType(eTokenType.Number);
            return words.Count(w => w.All(l => char.IsDigit(l)));

        }

        public int CalculateNumbersAsWords()
        {

            var words = tokens.GetValuesByTokenType(eTokenType.StringValue);
            var DigitStrings = ExtractWordsFromFile(NUMBERS_FILE);
            return words.Count(w => DigitStrings.Contains(w.ToLower()));
        }

        public double CalculatePercentageEmotionWords()
        {
            var words = tokens.GetValuesByTokenType(eTokenType.StringValue);
            var EmotionStrings = ExtractWordsFromFile(EMOTIOMS_FILE);
            return words.Count(w => EmotionStrings.Contains(w.ToLower())) / (double)words.Count();
        }

        public int CalculateQuestionWords()
        {
            var words = tokens.GetValuesByTokenType(eTokenType.StringValue);
            var QuestionStrings = ExtractWordsFromFile(QUESTIONS_FILE);
            return words.Count(w => QuestionStrings.Contains(w.ToLower()));
        }

        private List<string> ExtractWordsFromFile(string filename)
        {
            var filelines = File.ReadAllLines(filename);
            var strings = new List<string>();
            strings.AddRange(filelines.Select(line => line.ToLower()));
            return strings;
        }

    }
}
