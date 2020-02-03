using Crawler.Analyzers;
using Crawler.Exceptions;
using Crawler.LexicalAnalyzer;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Crawler.DeJargonizer;
using Xunit;

namespace CrawlerTests
{
	public class WordsAnalyzerTests
	{
		private readonly WordsAnalyzer wordsAnalyzer;

		public WordsAnalyzerTests()
		{
			wordsAnalyzer = new WordsAnalyzer(Mock.Of<IDeJargonizer>());
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
			var result = wordsAnalyzer.CalculateAverageLength(words.Select(w => new Token(eTokenType.StringValue, w)));

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
				() => wordsAnalyzer.CalculateWordsLengthStandardDeviation(new List<Token>{new Token(eTokenType.StringValue, It.IsAny<string>())})
			);
		}

		[Theory]
		[InlineData(0, "efe", "cse")]
		[InlineData(0.5, "s", "ff", "ss", "hh")]
		public void CalculateWordsLengthStandardDeviation_ShouldReturnWordsLengthStandardDeviation_WhenMoreThanOneWords(double expectedResult, params string[] words)
		{
			var result = wordsAnalyzer.CalculateWordsLengthStandardDeviation(words.Select(w => new Token(eTokenType.StringValue, w)));

			Assert.Equal(expectedResult, result);
		}
	}
}
