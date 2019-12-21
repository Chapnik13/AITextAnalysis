using Crawler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Crawler.LexicalAnalyzer;

namespace Crawler
{
	public class WordsAnalyzer
	{
		private DeJargonizer.DeJargonizer deJargonizer { get; set; }

		private IEnumerable<string> GetValuesByTokenType(IEnumerable<Token> tokens, eTokenType tokenType)
		{
			return tokens.Where(t => t.TokenType == tokenType).Select(t => t.Value);
		}

		public float CalculateAverageLength(IEnumerable<Token> tokens)
		{
			var words = GetValuesByTokenType(tokens, eTokenType.StringValue);

			return words.Any() ? (float)words.Average(w => w.Length) : 0;
		}

		public double CalculateStandardDeviation(IEnumerable<Token> tokens)
		{
			var words = GetValuesByTokenType(tokens, eTokenType.StringValue);

			if (words.Count() < 2) throw new StandardDeviationInvalidArgumentsAmountException();

			var average = CalculateAverageLength(tokens);
			var sumOfSquaresOfDifferences = words.Select(val => (val.Length - average) * (val.Length - average)).Sum();

			return Math.Sqrt(sumOfSquaresOfDifferences / (words.Count() - 1));
		}

		public int CalculateDeJargonizerScore(IEnumerable<Token> tokens)
		{
			var words = GetValuesByTokenType(tokens, eTokenType.StringValue);

			return deJargonizer.Analyze(words).Score;
		}

		public int CalculateDeJargonizerRareWordsPercentage(IEnumerable<Token> tokens)
		{
			var words = GetValuesByTokenType(tokens, eTokenType.StringValue);

			return deJargonizer.Analyze(words).RareWords.Count() / words.Count();
		}
	}
}
