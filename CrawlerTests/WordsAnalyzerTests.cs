using Crawler.Analyzers;
using Crawler.Exceptions;
using Crawler.LexicalAnalyzer;
using Moq;
using System;
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
		public void CalculateStandardDeviation_ShouldThrowStandardDeviationInvalidArgumentsAmountException_WhenEmptyList()
		{
			Assert.Throws<StandardDeviationInvalidArgumentsAmountException>(
				() => wordsAnalyzer.CalculateStandardDeviation(new List<Token>())
			);
		}

		[Fact]
		public void CalculateStandardDeviation_ShouldThrowStandardDeviationInvalidArgumentsAmountException_WhenOneWord()
		{
			Assert.Throws<StandardDeviationInvalidArgumentsAmountException>(
				() => wordsAnalyzer.CalculateStandardDeviation(new List<Token>{new Token(eTokenType.StringValue, It.IsAny<string>())})
			);
		}

		[Theory]
		[InlineData(0, "efe", "cse")]
		[InlineData(0.5, "s", "ff", "ss", "hh")]
		public void CalculateStandardDeviation_ShouldReturnWordsLengthStandardDeviation_WhenMoreThanOneWords(double expectedResult, params string[] words)
		{
			var result = wordsAnalyzer.CalculateStandardDeviation(words.Select(w => new Token(eTokenType.StringValue, w)));
			
			Assert.Equal(expectedResult, result);
		}

		[Theory]
		[InlineData(1, "111", "c5c")]
		[InlineData(2, "15", "33", "erf", "h8")]
		public void CalculateNumbersAsDigits_ShouldReturnNumberOfAppearences(double expectedResult, params string[] words)
		{
			var result = wordsAnalyzer.CalculateNumbersAsDigits(words.Select(w => new Token(eTokenType.StringValue, w)));
			Assert.Equal(expectedResult, result);
		}

		[Theory]
		[InlineData(2, "one", "three")]
		[InlineData(1, "four", "e1ee", "a3i", "nnn")]
		public void CalculateNumbersAsWords_ShouldReturnNumberOfAppearences(double expectedResult, params string[] words)
		{
			var result = wordsAnalyzer.CalculateNumbersAsWords(words.Select(w => new Token(eTokenType.StringValue, w)));
			Assert.Equal(expectedResult, result);
		}

		[Theory]
		[InlineData(1, "why", "to")]
		[InlineData(3, "where", "what", "whom", "hjh")]
		public void CalculateQuestionWords_ShouldReturnNumberOfAppearences(double expectedResult, params string[] words)
		{
			var result = wordsAnalyzer.CalculateQuestionWords(words.Select(w => new Token(eTokenType.StringValue, w)));
			Assert.Equal(expectedResult, result);
		}

		[Theory]
		[InlineData(0.5, "Unbelievable", "cse")]
		[InlineData(0.25, "Censored", "asd", "ss", "hh")]
		public void CalculateEmotionWords_ShouldReturnNumberOfAppearences(double expectedResult, params string[] words)
		{
			var result = (double)wordsAnalyzer.CalculateEmotionWords(words.Select(w => new Token(eTokenType.StringValue, w)));
			Assert.Equal(expectedResult, result);
		}
	}
}
