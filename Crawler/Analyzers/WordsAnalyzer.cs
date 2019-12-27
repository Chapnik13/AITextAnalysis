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
        private const string EmotionsFile = "Emotion.csv";
        private const string NumbersFile = "Numbers.csv";
        private const string QuestionsFile = "Questions.csv";

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
            var DigitStrings = new List<string>();
            var words = tokens.GetValuesByTokenType(eTokenType.StringValue);
            var Filelines = File.ReadAllLines(NumbersFile);
            DigitStrings.AddRange(Filelines.Select(line => line.ToLower()));
            return words.Count(w=> DigitStrings.Contains(w.ToLower()));

           

        }

        /// <summary>
        /// count numbers which appear as words
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public double CalculateEmotionWords(IEnumerable<Token> tokens)
        {
            var words = tokens.GetValuesByTokenType(eTokenType.StringValue);
            var Filelines = File.ReadAllLines(EmotionsFile);
            var EmotionStrings = new List<string>();
            EmotionStrings.AddRange(Filelines.Select(line => line.ToLower()));
            return ((double)(words.Count(w => EmotionStrings.Any(E => w.ToLower().Equals(E)))) / (double)words.Count()) ;
           
        }

        public int CalculateQuestionWords(IEnumerable<Token> tokens)
        {
            var words = tokens.GetValuesByTokenType(eTokenType.StringValue);
            var Filelines = File.ReadAllLines(QuestionsFile);
            var QuestionStrings = new List<string>();
            QuestionStrings.AddRange(Filelines.Select(line => line.ToLower()));

            return words.Count(w=>QuestionStrings.Contains(w.ToLower()));
            
        }
	}
}
