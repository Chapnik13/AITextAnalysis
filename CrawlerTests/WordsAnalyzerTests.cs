using Crawler.Analyzers.Helpers;
using Crawler.DeJargonizer;
using Crawler.Exceptions;
using Crawler.LexicalAnalyzer;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CrawlerTests
{
    public class WordsAnalyzerTests
    {
        private readonly IDeJargonizer deJargonizer;

        public WordsAnalyzerTests()
        {
            deJargonizer = Mock.Of<IDeJargonizer>();
        }

        [Fact]
        public void CalculateAverageLength_ShouldReturnZero_WhenEmptyList()
        {
            var result = new WordsAnalyzer(deJargonizer, new List<Token>()).CalculateAverageLength();

            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("gg")]
        [InlineData("aaa")]
        public void CalculateAverageLength_ShouldReturnWordsLength_WhenOneWord(string word)
        {
            var tokens = new List<Token> { new Token(eTokenType.StringValue, word) };
            var result = new WordsAnalyzer(deJargonizer, tokens).CalculateAverageLength();

            Assert.Equal(word.Length, result);
        }

        [Theory]
        [InlineData(4, "efe", "Secse")]
        [InlineData(5, "efeff", "Secse", "efdes")]
        public void CalculateAverageLength_SholudReturnWordsLengthAverage_WhenMoreThanOneWords(int expectedResult, params string[] words)
        {
            var tokens = words.Select(w => new Token(eTokenType.StringValue, w));
            var result = new WordsAnalyzer(deJargonizer, tokens).CalculateAverageLength();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CalculateStandardDeviation_ShouldThrowStandardDeviationInvalidArgumentsAmountException_WhenEmptyList()
        {
            Assert.Throws<StandardDeviationInvalidArgumentsAmountException>(
                () => new WordsAnalyzer(deJargonizer, new List<Token>()).CalculateStandardDeviation()
            );
        }

        [Fact]
        public void CalculateStandardDeviation_ShouldThrowStandardDeviationInvalidArgumentsAmountException_WhenOneWord()
        {
            var tokens = new List<Token> { new Token(eTokenType.StringValue, It.IsAny<string>()) };

            Assert.Throws<StandardDeviationInvalidArgumentsAmountException>(
                () => new WordsAnalyzer(deJargonizer, tokens).CalculateStandardDeviation()
            );
        }

        [Theory]
        [InlineData(0, "efe", "cse")]
        [InlineData(0.5, "s", "ff", "ss", "hh")]
        public void CalculateStandardDeviation_ShouldReturnWordsLengthStandardDeviation_WhenMoreThanOneWords(double expectedResult, params string[] words)
        {
            var tokens = words.Select(w => new Token(eTokenType.StringValue, w));
            var result = new WordsAnalyzer(deJargonizer, tokens).CalculateStandardDeviation();

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1, "111", "c5c")]
        [InlineData(2, "15", "33", "erf", "h8")]
        public void CalculateNumbersAsDigits_ShouldReturnNumberOfAppearences(double expectedResult, params string[] words)
        {
            var tokens = words.Select(w => new Token(eTokenType.Number, w));
            var result = new WordsAnalyzer(deJargonizer, tokens).CalculateNumbersAsDigits();

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(2, "one", "three")]
        [InlineData(1, "four", "e1ee", "a3i", "nnn")]
        public void CalculateNumbersAsWords_ShouldReturnNumberOfAppearences(double expectedResult, params string[] words)
        {
            var tokens = words.Select(w => new Token(eTokenType.StringValue, w));
            var result = new WordsAnalyzer(deJargonizer, tokens).CalculateNumbersAsWords();

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1, "why", "to")]
        [InlineData(3, "where", "what", "whom", "hjh")]
        public void CalculateQuestionWords_ShouldReturnNumberOfAppearences(double expectedResult, params string[] words)
        {
            var tokens = words.Select(w => new Token(eTokenType.StringValue, w));
            var result = new WordsAnalyzer(deJargonizer, tokens).CalculateQuestionWords();

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(0.5, "Unbelievable", "cse")]
        [InlineData(0.25, "Censored", "asd", "ss", "hh")]
        public void CalculateEmotionWords_ShouldReturnNumberOfAppearences(double expectedResult, params string[] words)
        {
            var tokens = words.Select(w => new Token(eTokenType.StringValue, w));
            var result = new WordsAnalyzer(deJargonizer, tokens).CalculatePercentageEmotionWords();

            Assert.Equal(expectedResult, result);
        }
    }
}
