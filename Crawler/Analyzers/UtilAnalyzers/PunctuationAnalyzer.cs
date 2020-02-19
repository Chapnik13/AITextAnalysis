using Crawler.Configs;
using Crawler.Exceptions;
using Crawler.LexicalAnalyzer;
using Crawler.PartOfSpeechTagger;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Crawler.Analyzers.UtilAnalyzers
{
    public class PunctuationAnalyzer : IPunctuationAnalyzer
    {
	    private readonly IOptions<DataFilesConfig> config;

	    public PunctuationAnalyzer(IOptions<DataFilesConfig> dataFilesConfig)
	    {
		    this.config = dataFilesConfig;
	    }

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

        public double CalculatePassiveVoiceSentencesPercentage(List<PosTagToken> posTagTokens)
        {
	        var toBeForms = ExtractWordsFromFile(config.Value.ToBeFormsFile);
	        var passiveVoiceSentencesCount = 0;
	        var indexOfNextSentenceEnd = posTagTokens.FindIndex(token => token.ExtendedType == ".");
            var totalSentencesCount = posTagTokens.Count(token => token.ExtendedType == ".");

	        if (totalSentencesCount == 0)
	        {
		        totalSentencesCount = 1;
	        }

	        while (indexOfNextSentenceEnd != -1)
	        {
		        var sentence = posTagTokens.Take(indexOfNextSentenceEnd).ToList();

		        var indexOfToBeForm = sentence.FindIndex(token => toBeForms.Contains(token.Value));
		        var indexOfPastParticiple = sentence.FindIndex(token => token.ExtendedType == "VBN");

		        var isPassiveVoiceSentence = indexOfToBeForm != -1 && indexOfPastParticiple != -1 && indexOfToBeForm < indexOfPastParticiple;

		        if (isPassiveVoiceSentence)
		        {
			        passiveVoiceSentencesCount++;
		        }

		        posTagTokens = posTagTokens.Skip(indexOfNextSentenceEnd + 1).ToList();
                indexOfNextSentenceEnd = posTagTokens.FindIndex(token => token.ExtendedType == ".");
            }

	        return passiveVoiceSentencesCount / (double)totalSentencesCount;
        }

        private IEnumerable<string> ExtractWordsFromFile(string filename)
        {
	        var filelines = File.ReadAllLines(filename);

	        return filelines.Select(line => line.ToLower());
        }
    }
}
