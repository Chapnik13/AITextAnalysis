using Crawler.Configs;
using Crawler.DeJargonizer;
using Crawler.Exceptions;
using Crawler.ExtensionMethods;
using Crawler.LexicalAnalyzer;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Crawler.Analyzers.UtilAnalyzers
{
    public class WordsAnalyzer : IWordsAnalyzer
    {
        private readonly string emotionsFilePath;
        private readonly string numbersFilePath;
        private readonly string questionsFilePath;

        private readonly IDeJargonizer deJargonizer;

        public WordsAnalyzer(IDeJargonizer deJargonizer, IOptions<DataFilesConfig> dataFilesConfig)
        {
            this.deJargonizer = deJargonizer;
            emotionsFilePath = dataFilesConfig.Value.EmotionsFile;
            numbersFilePath = dataFilesConfig.Value.NumbersFile;
            questionsFilePath = dataFilesConfig.Value.QuestionsFile;
        }

        public int CountWords(List<Token> tokens)
        {
            return tokens.Count(t => t.TokenType == eTokenType.StringValue || t.TokenType == eTokenType.Number);
        }

        public float CalculateAverageLength(List<Token> tokens)
        {
            var words = tokens.GetValuesByTokenTypes(eTokenType.StringValue);

            return words.Any() ? (float)words.Average(w => w.Length) : 0;
        }

        public double CalculateWordsLengthStandardDeviation(List<Token> tokens)
        {
            var words = tokens.GetValuesByTokenTypes(eTokenType.StringValue);

            if (words.Count() < 2) throw new StandardDeviationInvalidArgumentsAmountException();

            var average = CalculateAverageLength(tokens);
            var sumOfSquaresOfDifferences = words.Select(val => Math.Pow(val.Length - average, 2)).Sum();

            return Math.Sqrt(sumOfSquaresOfDifferences / (words.Count() - 1));
        }

        public DeJargonizerResult CalculateDeJargonizer(List<Token> tokens)
        {
            var words = tokens.GetValuesByTokenTypes(eTokenType.StringValue);

            return deJargonizer.Analyze(words);
        }

        public int CalculateNumbersAsDigits(List<Token> tokens)
        {
            var words = tokens.GetValuesByTokenTypes(eTokenType.Number);

            return words.Count(w => w.All(l => char.IsDigit(l)));

        }

        public int CalculateNumbersAsWords(List<Token> tokens)
        {
            var words = tokens.GetValuesByTokenTypes(eTokenType.StringValue);
            var digitStrings = ExtractWordsFromFile(numbersFilePath);

            return words.Count(w => digitStrings.Contains(w.ToLower().RemoveApostrophe()));
        }

        public double CalculatePercentageEmotionWords(List<Token> tokens)
        {
            var words = tokens.GetValuesByTokenTypes(eTokenType.StringValue);
            var emotionStrings = ExtractWordsFromFile(emotionsFilePath);

            return words.Count(w => emotionStrings.Contains(w.ToLower().RemoveApostrophe())) / (double)words.Count();
        }

        public int CalculateQuestionWords(List<Token> tokens)
        {
            var words = tokens.GetValuesByTokenTypes(eTokenType.StringValue);
            var questionStrings = ExtractWordsFromFile(questionsFilePath);

            return words.Count(w => questionStrings.Contains(w.ToLower().RemoveApostrophe()));
        }

        private IEnumerable<string> ExtractWordsFromFile(string filename)
        {
            var filelines = File.ReadAllLines(filename);
            return filelines.Select(line => line.ToLower());
        }

    }
}
