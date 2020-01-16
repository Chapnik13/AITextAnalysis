using System;
using System.Collections.Generic;
using System.Linq;
using Crawler.DeJargonizer;
using Crawler.Exceptions;
using Crawler.ExtensionMethods;
using Crawler.LexicalAnalyzer;
using System.IO;

namespace Crawler.Analyzers
{
	public class WordsAnalyzer : IWordsAnalyzer
	{
        private const string EMOTIOMS_FILE = "Emotion.csv";
        private const string NUMBERS_FILE = "Numbers.csv";
        private const string QUESTIONS_FILE = "Questions.csv";

        private IDeJargonizer deJargonizer;

		public WordsAnalyzer(IDeJargonizer deJargonizer)
		{
			this.deJargonizer = deJargonizer;
		}

		public float CalculateAverageLength(IEnumerable<Token> tokens)
		{
			var words = tokens.GetValuesByTokenType(eTokenType.StringValue);

			return words.Any() ? (float)words.Average(w => w.Length) : 0;
		}

		public double CalculateWordsLengthStandardDeviation(IEnumerable<Token> tokens)
		{
			var words = tokens.GetValuesByTokenType(eTokenType.StringValue);

			if (words.Count() < 2) throw new StandardDeviationInvalidArgumentsAmountException();

			var average = CalculateAverageLength(tokens);
			var sumOfSquaresOfDifferences = words.Select(val => Math.Pow(val.Length - average, 2)).Sum();

			return Math.Sqrt(sumOfSquaresOfDifferences / (words.Count() - 1));
		}

		public DeJargonizerResult CalculateDeJargonizer(IEnumerable<Token> tokens)
		{
			var words = tokens.GetValuesByTokenType(eTokenType.StringValue);

			return deJargonizer.Analyze(words);
		}

		/// <summary>
        /// 
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public int CalculateNumbersAsDigits(IEnumerable<Token> tokens)
        {
            var words = tokens.GetValuesByTokenType(eTokenType.Number);
            return words.Count(w => w.All(l => char.IsDigit(l)));
            
        }

        /// <summary>
        /// count numbers which appear as digits
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public int CalculateNumbersAsWords(IEnumerable<Token> tokens)
        {
            
            var words = tokens.GetValuesByTokenType(eTokenType.StringValue);
            var DigitStrings = ExtractWordsFromFile(NUMBERS_FILE);
            return words.Count(w=>DigitStrings.Contains(w.ToLower()));
        }

        /// <summary>
        /// count numbers which appear as words
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public double CalculatePercentageEmotionWords(IEnumerable<Token> tokens)
        {
            var words = tokens.GetValuesByTokenType(eTokenType.StringValue);
            var EmotionStrings = ExtractWordsFromFile(EMOTIOMS_FILE);
            return (double)((double)(words.Count(w=>EmotionStrings.Contains(w.ToLower()))) / (double)words.Count()) ; 
        }

        public int CalculateQuestionWords(IEnumerable<Token> tokens)
        {
            var words = tokens.GetValuesByTokenType(eTokenType.StringValue);
            var QuestionStrings = ExtractWordsFromFile(QUESTIONS_FILE);
            return words.Count(w=>QuestionStrings.Contains(w.ToLower()));  
        }

        private List<string> ExtractWordsFromFile(string filename)
        {
            var Filelines = File.ReadAllLines(filename);
            var Strings = new List<string>();
            Strings.AddRange(Filelines.Select(line => line.ToLower()));
            return Strings;
        }

	}
}
