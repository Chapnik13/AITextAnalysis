using Crawler.LexicalAnalyzer;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;
using Xunit;

namespace CrawlerTests
{
    public class LexerTests
    {
        private readonly ILexer lexer;

        public LexerTests()
        {
            lexer = new Lexer(Mock.Of<ILogger<Lexer>>());
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public void GetTokens_ShouldReturnEmptyEnumerable_WhenTextIsEmpty(string text)
        {
            Assert.Empty(lexer.GetTokens(text));
        }

        [Theory]
        [InlineData("Hello", 1)]
        [InlineData("Hello World", 2)]
        [InlineData("Hello12", 2)]
        public void GetTokens_ShouldReturnMultipleTokens_WhenTextContainsMultipleWords(string text, int amountOfTokens)
        {
            Assert.Equal(amountOfTokens, lexer.GetTokens(text).Count());
        }

        [Theory]
        [InlineData("Hello")]
        [InlineData("Bye[]12")]
        public void GetTokens_ShouldContainStringValueToken_WhenGivenTextWithStringValue(string text)
        {
            var result = lexer.GetTokens(text);

            Assert.Contains(result, token => token.TokenType == eTokenType.StringValue);
        }

        [Theory]
        [InlineData("Hello", "Hello")]
        [InlineData("Bye[]12", "Bye")]
        public void GetTokens_ShouldContainCorrectStringValueToken_WhenGivenTextWithStringValue(string text, string tokenValue)
        {
            var result = lexer.GetTokens(text);
            var expectedToken = new Token(eTokenType.StringValue, tokenValue);

            Assert.Contains(result, token => token.Equals(expectedToken));
        }

        [Theory]
        [InlineData("12")]
        [InlineData("Bye[]12")]
        public void GetTokens_ShouldContainNumberToken_WhenGivenTextWithNumber(string text)
        {
            var result = lexer.GetTokens(text);

            Assert.Contains(result, token => token.TokenType == eTokenType.Number);
        }


        [Theory]
        [InlineData("12", "12")]
        [InlineData("Bye[]12", "12")]
        public void GetTokens_ShouldContainCorrectNumberToken_WhenGivenTextWithNumber(string text, string tokenValue)
        {
            var result = lexer.GetTokens(text);
            var expectedToken = new Token(eTokenType.Number, tokenValue);

            Assert.Contains(result, token => token.Equals(expectedToken));
        }

        [Theory]
        [InlineData("Hello, World")]
        [InlineData("Bye.12")]
        public void GetTokens_ShouldContainPunctuationToken_WhenGivenTextWithPunctuation(string text)
        {
            var result = lexer.GetTokens(text);

            Assert.Contains(result, token => token.TokenType == eTokenType.Punctuation);
        }


        [Theory]
        [InlineData("Hello, World", ",")]
        [InlineData("Bye.,12", ".")]
        public void GetTokens_ShouldContainCorrectPunctuationToken_WhenGivenTextWithPunctuation(string text, string tokenValue)
        {
            var result = lexer.GetTokens(text);
            var expectedToken = new Token(eTokenType.Punctuation, tokenValue);

            Assert.Contains(result, token => token.Equals(expectedToken));
        }
    }
}
