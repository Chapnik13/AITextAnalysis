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

		public double CalculateStandardDeviation(IEnumerable<Token> tokens)
		{
			var words = tokens.GetValuesByTokenType(eTokenType.StringValue);

			if (words.Count() < 2) throw new StandardDeviationInvalidArgumentsAmountException();

			var average = CalculateAverageLength(tokens);
			var sumOfSquaresOfDifferences = words.Select(val => (val.Length - average) * (val.Length - average)).Sum();

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
            var words = tokens.GetValuesByTokenType(eTokenType.StringValue);
            return words.Count(w => w.All(l => char.IsDigit(l)));
            
        }

        /// <summary>
        /// count numbers which appear as digits
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public int CalculateNumbersAsWords(IEnumerable<Token> tokens)
        {
            var DigitStrings = new List<string>() { "zero", "one", "two", "three", "four", "five", "six", "seven", 
                "eight", "nine","ten","eleven","twelve","thirteen","fourteen","fifteen","sixteen","seventeen",
                "eighteen", "nineteen", "twenty","thirty","fourty", "fifty","sixty","seventy","eighty","ninty",
                "hundred","thousand","million","billion" };
            var words = tokens.GetValuesByTokenType(eTokenType.StringValue);
            return words.Count(w=>DigitStrings.Any(D=>w.ToLower().Equals(D)));
           

        }

        /// <summary>
        /// count numbers which appear as words
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public double CalculateEmotionWords(IEnumerable<Token> tokens)
        {
            var words = tokens.GetValuesByTokenType(eTokenType.StringValue);
            var Filelines = File.ReadAllLines("Emotion.csv");
            var EmotionStrings = new List<string>(){};

            foreach(string line in Filelines)
            {
                EmotionStrings.Add(line.ToLower());
            }

            return ((double)(words.Count(w => EmotionStrings.Any(E => w.ToLower().Equals(E)))) / (double)words.Count()) ;
        }

        public int CalculateQuestionWords(IEnumerable<Token> tokens)
        {
            var words = tokens.GetValuesByTokenType(eTokenType.StringValue);
            var QuestionStrings = new List<string>(){"what","when","where","which","who","whom","whose","why", "how" ,"how far",
                "how long","how many","how much","how old"};

            
            return words.Count(w=>QuestionStrings.Any(E=>w.Equals(E)));
            
        }
	}
}
