using System;
using System.Collections.Generic;
using System.Linq;
using Crawler.DeJargonizer;
using Crawler.Exceptions;
using Crawler.ExtensionMethods;
using Crawler.LexicalAnalyzer;

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
	}
}
