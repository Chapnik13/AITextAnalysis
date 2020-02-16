using Crawler.Analyzers.UtilAnalyzers;
using Crawler.Configs;
using Crawler.DeJargonizer;
using Crawler.Exceptions;
using Crawler.LexicalAnalyzer;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CrawlerTests
{
    public class WordsAnalyzerTests
    {
        private readonly WordsAnalyzer wordsAnalyzer;

        public WordsAnalyzerTests()
        {
            var dataFilesConfigOptions = Mock.Of<IOptions<DataFilesConfig>>();
            var dataFilesConfig = new DataFilesConfig
            {
                EmotionsFile = "data/Emotion.csv",
                NumbersFile = "data/Numbers.csv",
                QuestionsFile = "data/Questions.csv"
            };

            Mock.Get(dataFilesConfigOptions)
                .Setup(options => options.Value)
                .Returns(dataFilesConfig);

            wordsAnalyzer = new WordsAnalyzer(Mock.Of<IDeJargonizer>(), dataFilesConfigOptions);
        }

        [Fact]
        public void CalculateAverageLength_ShouldReturnZero_WhenEmptyList()
        {
            var result = wordsAnalyzer.CalculateAverageLength(new List<Token>());

            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("gg")]
        [InlineData("aaa")]
        public void CalculateAverageLength_ShouldReturnWordsLength_WhenOneWord(string word)
        {
            var result = wordsAnalyzer.CalculateAverageLength(new List<Token> { new Token(eTokenType.StringValue, word) });

            Assert.Equal(word.Length, result);
        }

        [Theory]
        [InlineData(4, "efe", "Secse")]
        [InlineData(5, "efeff", "Secse", "efdes")]
        public void CalculateAverageLength_SholudReturnWordsLengthAverage_WhenMoreThanOneWords(int expectedResult, params string[] words)
        {
            var result = wordsAnalyzer.CalculateAverageLength(words.Select(w => new Token(eTokenType.StringValue, w)).ToList());

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CalculateWordsLengthStandardDeviation_ShouldThrowStandardDeviationInvalidArgumentsAmountException_WhenEmptyList()
        {
            Assert.Throws<StandardDeviationInvalidArgumentsAmountException>(
                () => wordsAnalyzer.CalculateWordsLengthStandardDeviation(new List<Token>())
            );
        }

        [Fact]
        public void CalculateWordsLengthStandardDeviation_ShouldThrowStandardDeviationInvalidArgumentsAmountException_WhenOneWord()
        {
            Assert.Throws<StandardDeviationInvalidArgumentsAmountException>(
                () => wordsAnalyzer.CalculateWordsLengthStandardDeviation(new List<Token> { new Token(eTokenType.StringValue, It.IsAny<string>()) })
            );
        }

        [Theory]
        [InlineData(0, "efe", "cse")]
        [InlineData(0.5, "s", "ff", "ss", "hh")]
        public void CalculateWordsLengthStandardDeviation_ShouldReturnWordsLengthStandardDeviation_WhenMoreThanOneWords(double expectedResult, params string[] words)
        {
            var result = wordsAnalyzer.CalculateWordsLengthStandardDeviation(words.Select(w => new Token(eTokenType.StringValue, w)).ToList());

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1, "111", "c5c")]
        [InlineData(2, "15", "33", "erf", "h8")]
        public void CalculateNumbersAsDigits_ShouldReturnNumberOfAppearences(double expectedResult, params string[] words)
        {
            var result = wordsAnalyzer.CalculateNumbersAsDigits(words.Select(w => new Token(eTokenType.Number, w)).ToList());
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(2, "one", "three")]
        [InlineData(1, "four", "e1ee", "a3i", "nnn")]
        public void CalculateNumbersAsWords_ShouldReturnNumberOfAppearences(double expectedResult, params string[] words)
        {
            var result = wordsAnalyzer.CalculateNumbersAsWords(words.Select(w => new Token(eTokenType.StringValue, w)).ToList());
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1, "why", "to")]
        [InlineData(3, "where", "what", "whom", "hjh")]
        public void CalculateQuestionWords_ShouldReturnNumberOfAppearences(double expectedResult, params string[] words)
        {
            var result = wordsAnalyzer.CalculateQuestionWords(words.Select(w => new Token(eTokenType.StringValue, w)).ToList());
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(0.5, "Unbelievable", "cse")]
        [InlineData(0.25, "Censored", "asd", "ss", "hh")]
        public void CalculateEmotionWords_ShouldReturnNumberOfAppearences(double expectedResult, params string[] words)
        {
            var result = (double)wordsAnalyzer.CalculatePercentageEmotionWords(words.Select(w => new Token(eTokenType.StringValue, w)).ToList());
            Assert.Equal(expectedResult, result);
        }
    }
}