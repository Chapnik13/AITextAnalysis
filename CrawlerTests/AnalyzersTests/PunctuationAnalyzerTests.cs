using Crawler.Analyzers;
using Crawler.LexicalAnalyzer;
using System.Collections.Generic;
using System.Linq;
using Crawler.Configs;
using Crawler.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace CrawlerTests
{
	public class PunctuationAnalyzerTests
	{
		private readonly ILexer lexer;
		private readonly LexerConfig config;
		private readonly PunctuationAnalyzer punctuationsAnalyzer = new PunctuationAnalyzer();

		public PunctuationAnalyzerTests()
		{
			var configOptions = Mock.Of<IOptions<LexerConfig>>();

			config = new LexerConfig
			{
				TokensDefinitions = new[]
				{
					new TokenDefinition {TokenType = eTokenType.StringValue, Pattern = "^[a-zA-Z]+"},
					new TokenDefinition {TokenType = eTokenType.Number, Pattern = "^[0-9]+"},
					new TokenDefinition {TokenType = eTokenType.Punctuation, Pattern = "^[,\\.]"}
				}
			};

			Mock.Get(configOptions).Setup(c => c.Value).Returns(config);

			lexer = new Lexer(Mock.Of<ILogger<Lexer>>(), configOptions);
		}

		[Fact]
		public void CalculateAverageWordCountBetweenPunctuations_ShouldReturnZero_WhenEmptyList()
		{
			var result = punctuationsAnalyzer.CalculateAverageWordsCountBetweenPunctuation(new List<Token>());

			Assert.Equal(0, result);
		}

		[Fact]
		public void CalculateAverageWordCountBetweenPunctuations_ShouldReturnZero_WhenNoPunctuations()
		{
			var result = punctuationsAnalyzer.CalculateAverageWordsCountBetweenPunctuation(new List<Token>
				{new Token(eTokenType.StringValue, "gg")});

			Assert.Equal(0, result);
		}

		[Theory]
		[InlineData(1, ",fcbdrhdt,")]
		[InlineData(3, ",gg bu bg ,")]
		[InlineData(5, ", segs sgrsgs sgs sg hh,")]
		public void CalculateAverageWordCountBetweenPunctuations_ShouldReturnWordsCount_WhenOnePunctuationsPair(
			int expectedResult, string sentence)
		{
			var result =
				punctuationsAnalyzer.CalculateAverageWordsCountBetweenPunctuation(lexer.GetTokens(sentence).ToList());

			Assert.Equal(expectedResult, result);
		}

		[Theory]
		[InlineData(1, ",fcbdrhdt, sefsfesf,")]
		[InlineData(2, ",gg bu bg , fdg.")]
		[InlineData(4, ", segs sgrsgs sgs sg hh, gn gg gh.")]
		public void
			CalculateAverageWordCountBetweenPunctuations_ShouldReturnWordsCountAverage_WhenMoreThanOnePunctuationsPair(
				int expectedResult, string sentence)
		{
			var result =
				punctuationsAnalyzer.CalculateAverageWordsCountBetweenPunctuation(lexer.GetTokens(sentence).ToList());

			Assert.Equal(expectedResult, result);
		}

		[Fact]
		public void CalculateMaxWordCountBetweenPunctuations_ShouldReturnZero_WhenEmptyList()
		{
			var result = punctuationsAnalyzer.CalculateMaxWordsCountBetweenPunctuation(new List<Token>());

			Assert.Equal(0, result);
		}

		[Fact]
		public void CalculateMaxWordCountBetweenPunctuations_ShouldReturnZero_WhenNoPunctuations()
		{
			var result = punctuationsAnalyzer.CalculateMaxWordsCountBetweenPunctuation(new List<Token>
				{new Token(eTokenType.StringValue, "gg")});

			Assert.Equal(0, result);
		}

		[Theory]
		[InlineData(1, ",fcbdrhdt,")]
		[InlineData(3, ",gg bu bg ,")]
		[InlineData(5, ", segs sgrsgs sgs sg hh,")]
		public void CalculateMaxWordCountBetweenPunctuations_ShouldReturnWordsCount_WhenOnePunctuationsPair(
			int expectedResult, string sentence)
		{
			var result =
				punctuationsAnalyzer.CalculateMaxWordsCountBetweenPunctuation(lexer.GetTokens(sentence).ToList());

			Assert.Equal(expectedResult, result);
		}

		[Theory]
		[InlineData(2, ",fcbdrhdt, sefsf esf,")]
		[InlineData(3, ",gg bu bg , fdg.")]
		[InlineData(5, ", segs sgrsgs sgs sg hh, gn gg gh.")]
		public void CalculateMaxWordCountBetweenPunctuations_ShouldReturnMaxWordsCount_WhenMoreThanOnePunctuationsPair(
			int expectedResult, string sentence)
		{
			var result =
				punctuationsAnalyzer.CalculateMaxWordsCountBetweenPunctuation(lexer.GetTokens(sentence).ToList());

			Assert.Equal(expectedResult, result);
		}

		[Fact]
		public void
			CalculateStandardDeviation_ShouldThrowStandardDeviationInvalidArgumentsAmountException_WhenEmptyList()
		{
			Assert.Throws<StandardDeviationInvalidArgumentsAmountException>(
				() => punctuationsAnalyzer.CalculateWordsCountsBetweenPunctuationStandardDeviation(new List<Token>())
			);
		}

		[Fact]
		public void CalculateStandardDeviation_ShouldThrowStandardDeviationInvalidArgumentsAmountException_WhenOneWord()
		{
			Assert.Throws<StandardDeviationInvalidArgumentsAmountException>(
				() => punctuationsAnalyzer.CalculateWordsCountsBetweenPunctuationStandardDeviation(
					new List<Token>
					{
						new Token(eTokenType.StringValue, It.IsAny<string>())
					})
			);
		}

		[Theory]
		[InlineData(0, ",efe, cse.")]
		[InlineData(0.5, ", sf, wf regt, rth umu. aede werg,")]
		public void CalculateStandardDeviation_ShouldReturnWordsLengthStandardDeviation_WhenMoreThanOneWords(
			double expectedResult, string sentence)
		{
			var result =
				punctuationsAnalyzer.CalculateWordsCountsBetweenPunctuationStandardDeviation(lexer.GetTokens(sentence).ToList());

			Assert.Equal(expectedResult, result);
		}

		[Fact]
		public void CalculateWordsCountNinthDecile_ShouldReturnThrowDecileInvalidArgumentsAmountException_WhenEmptyList()
		{
			Assert.Throws<DecileInvalidArgumentsAmountException>(
				() => punctuationsAnalyzer.CalculateWordsCountDecile(9, new List<Token>())
			);
		}

		[Theory]
		[InlineData(",efe, cse.")]
		[InlineData(", sf, wf regt, rth umu. aede werg,")]
		public void CalculateWordsCountNinthDecile_ShouldReturnThrowDecileInvalidArgumentsAmountException_WhenLessThanTenPhrases(string sentence)
		{
			Assert.Throws<DecileInvalidArgumentsAmountException>(
				() => punctuationsAnalyzer.CalculateWordsCountDecile(9, lexer.GetTokens(sentence).ToList())
			);
		}

		[Fact]
		public void CalculateWordsCountNinthDecile_ShouldReturnThrowDecileInvalidArgumentsAmountException_WhenMoreThanTenPhrases()
		{
			var sentence = ",gg, dd, gg, cc, ss, vv, dd, rr, tt, ff ggg fff,";

			var result = punctuationsAnalyzer.CalculateWordsCountDecile(9, lexer.GetTokens(sentence).ToList());

			Assert.Equal(3, result);
		}
	}
}