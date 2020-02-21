using Crawler.Exceptions;
using Crawler.LexicalAnalyzer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Crawler.Analyzers.UtilAnalyzers
{
    public class PunctuationAnalyzer : IPunctuationAnalyzer
    {
        public double CalculateAverageWordsCountBetweenPunctuation(List<Token> tokens)
        {
            if (!tokens.Any()) return 0;

            var wordsCountBetweenPunctuations = WordsCountBetweenPunctuations(tokens);

            return !wordsCountBetweenPunctuations.Any() ? 0 : wordsCountBetweenPunctuations.Average();
        }

        public int CalculateMaxWordsCountBetweenPunctuation(List<Token> tokens)
        {
            if (!tokens.Any()) return 0;

            var wordsCountBetweenPunctuations = WordsCountBetweenPunctuations(tokens);

            return !wordsCountBetweenPunctuations.Any() ? 0 : wordsCountBetweenPunctuations.Max();
        }

        public double CalculateWordsCountBetweenPunctuationStandardDeviation(List<Token> tokens)
        {
            var wordsCountBetweenPunctuations = WordsCountBetweenPunctuations(tokens);

            if (wordsCountBetweenPunctuations.Count < 2) throw new StandardDeviationInvalidArgumentsAmountException();

            var average = CalculateAverageWordsCountBetweenPunctuation(tokens);
            var sumOfSquaresOfDifferences = wordsCountBetweenPunctuations.Select(val => Math.Pow(val - average, 2)).Sum();

            return Math.Sqrt(sumOfSquaresOfDifferences / (wordsCountBetweenPunctuations.Count - 1));
        }

        public int CalculateWordsCountDecile(int decile, List<Token> tokens)
        {
            var wordsCountBetweenPunctuations = WordsCountBetweenPunctuations(tokens);

            if (wordsCountBetweenPunctuations.Count < 10) throw new DecileInvalidArgumentsAmountException();

            wordsCountBetweenPunctuations.Sort();

            var decileIndex = (int)Math.Ceiling((decile * (wordsCountBetweenPunctuations.Count + 1)) / 10.0);

            return wordsCountBetweenPunctuations[decileIndex - 1];
        }

        public int CountCharacter(char chr, List<Token> tokens)
        {
            return tokens.Count(t => t.TokenType == eTokenType.Punctuation && t.Value[0] == chr);
        }

        private List<int> WordsCountBetweenPunctuations(List<Token> tokens)
        {
            var wordsCount = new List<int>();
            var currentWordsCount = 0;

            foreach (var token in tokens)
            {
                if (token.TokenType == eTokenType.Punctuation && (token.Value == "." || token.Value == ","))
                {
                    if (currentWordsCount != 0)
                    {
                        wordsCount.Add(currentWordsCount);
                    }

                    currentWordsCount = 0;
                }
                else if (token.TokenType == eTokenType.StringValue)
                {
                    currentWordsCount++;
                }
            }

            return wordsCount;
        }
    }
}
