using System;
using Crawler.LexicalAnalyzer;
using System.Collections.Generic;
using System.Linq;
using Crawler.Exceptions;

namespace Crawler.Analyzers
{
	public class PunctuationAnalyzer : IPunctuationAnalyzer
	{
		public double CalculateAverageWordsCountBetweenPunctuation(List<Token> tokens)
		{
			if(!tokens.Any()) return 0;

			var wordsCountsBetweenPunctuations = WordsCountsBetweenPunctuations(tokens);

			return !wordsCountsBetweenPunctuations.Any() ? 0 : wordsCountsBetweenPunctuations.Average();
		}

		private List<int> WordsCountsBetweenPunctuations(List<Token> tokens)
		{
			var wordsCounts = new List<int>();
			var currentWordsCount = 0;

			foreach (var token in tokens)
			{
				if (token.TokenType == eTokenType.Punctuation)
				{
					if (currentWordsCount != 0)
					{
						wordsCounts.Add(currentWordsCount);
					}

					currentWordsCount = 0;
				}
				else
				{
					currentWordsCount++;
				}
			}

			return wordsCounts;
		}

		public double CalculateMaxWordsCountBetweenPunctuation(List<Token> tokens)
		{
			if (!tokens.Any()) return 0;

			var wordsCountsBetweenPunctuations = WordsCountsBetweenPunctuations(tokens);

			return !wordsCountsBetweenPunctuations.Any() ? 0 : wordsCountsBetweenPunctuations.Max();
		}

		public double CalculateWordsCountsBetweenPunctuationStandardDeviation(List<Token> tokens)
		{
			var wordsCountsBetweenPunctuations = WordsCountsBetweenPunctuations(tokens);

			if (wordsCountsBetweenPunctuations.Count < 2) throw new StandardDeviationInvalidArgumentsAmountException();

			var average = CalculateAverageWordsCountBetweenPunctuation(tokens);
			var sumOfSquaresOfDifferences = wordsCountsBetweenPunctuations.Select(val => Math.Pow(val - average, 2)).Sum();

			return Math.Sqrt(sumOfSquaresOfDifferences / (wordsCountsBetweenPunctuations.Count - 1));
		}

		public int CalculateWordsCountDecile(int decile, List<Token> tokens)
		{
			var wordsCountsBetweenPunctuations = WordsCountsBetweenPunctuations(tokens);

			if (wordsCountsBetweenPunctuations.Count < 10) throw new DecileInvalidArgumentsAmountException();

			wordsCountsBetweenPunctuations.Sort();

			var decileIndex = (int)Math.Ceiling((decile * (wordsCountsBetweenPunctuations.Count + 1)) / (double)10);

			return wordsCountsBetweenPunctuations[decileIndex - 1];
		}
	}
}
