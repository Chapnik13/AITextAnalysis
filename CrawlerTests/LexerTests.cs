using System.Collections.Generic;
using Crawler.LexicalAnalyzer;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;
using Crawler.Configs;
using Microsoft.Extensions.Options;
using Xunit;

namespace CrawlerTests
{
    public class LexerTests
    {
        private readonly ILexer lexer;
        private readonly LexerConfig config;

        public LexerTests()
        {
            var options = Mock.Of<IOptions<LexerConfig>>();
            config = new LexerConfig();

            Mock.Get(options).Setup(c => c.Value).Returns(config);


            lexer = new Lexer(Mock.Of<ILogger<Lexer>>(), options);
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
            config.TokensDefinitions = new[]
            {
                new TokenDefinition { TokenType = eTokenType.StringValue, Pattern = "^[a-zA-Z]+" },
                new TokenDefinition { TokenType = eTokenType.Number, Pattern = "^[0-9]+" }
            };

            Assert.Equal(amountOfTokens, lexer.GetTokens(text).Count());
        }

        [Theory]
        [InlineData("Hello")]
        [InlineData("Bye[]12")]
        public void GetTokens_ShouldContainStringValueToken_WhenGivenTextWithStringValue(string text)
        {
            config.TokensDefinitions = new[]
            {
                new TokenDefinition { TokenType = eTokenType.StringValue, Pattern = "^[a-zA-Z]+" }
            };

            var result = lexer.GetTokens(text);

            Assert.Contains(result, token => token.TokenType == eTokenType.StringValue);
        }

        [Theory]
        [InlineData("12")]
        [InlineData("Bye[]12")]
        public void GetTokens_ShouldContainNumberToken_WhenGivenTextWithNumber(string text)
        {
            config.TokensDefinitions = new[]
            {
                new TokenDefinition { TokenType = eTokenType.Number, Pattern = "^[0-9]+" }
            };

            var result = lexer.GetTokens(text);

            Assert.Contains(result, token => token.TokenType == eTokenType.Number);
        }

        [Theory]
        [InlineData("Hello, World")]
        [InlineData("Bye.12")]
        public void GetTokens_ShouldContainPunctuationToken_WhenGivenTextWithPunctuation(string text)
        {
            config.TokensDefinitions = new[]
            {
                new TokenDefinition { TokenType = eTokenType.Punctuation, Pattern = "^[,\\.]" }
            };

            var result = lexer.GetTokens(text);

            Assert.Contains(result, token => token.TokenType == eTokenType.Punctuation);
        }
    }
}
