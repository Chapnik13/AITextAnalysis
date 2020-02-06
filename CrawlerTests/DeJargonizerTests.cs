using System.Collections.Generic;
using Crawler.Configs;
using Crawler.DeJargonizer;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace CrawlerTests
{
	public class DeJargonizerTests
	{
		private readonly IWordsCountLoader wordsCountLoader;
		private readonly DeJargonizeAnalyzer deJargonizer;

		public DeJargonizerTests()
		{
			wordsCountLoader = Mock.Of<IWordsCountLoader>();

			Mock.Get(wordsCountLoader)
				.Setup(w => w.Load())
				.Returns(new Dictionary<string, int>
				{
					{ "carley", 18 },
					{ "pare", 45 },
					{ "analyze", 548 },
					{ "banner", 692 },
					{ "scout", 346 },
					{ "to", 1561 },
					{ "and", 4159 },
					{ "result", 1541 }
				});

			var wordsCountThresholdsConfigOptions = Mock.Of<IOptions<WordsCountThresholdsConfig>>();

			Mock.Get(wordsCountThresholdsConfigOptions)
				.Setup(w => w.Value)
				.Returns(new WordsCountThresholdsConfig {CommonWordsThreshold = 1000, NormalWordsThreshold = 50});

			deJargonizer = new DeJargonizeAnalyzer(wordsCountLoader,wordsCountThresholdsConfigOptions
			);
		}

		[Theory]
		[InlineData("to", "scout", "banner", "rfrf", "to")]
		[InlineData("and", "analyze", "and", "wegfewwf", "rfrf", "WEfewgt")]
		[InlineData("result", "result")]
		public void GetCommonWords_ShouldReturnOnlyCommonWord_WhenGettingMultipleWords(string expectedWord, params string[] words)
		{
			var result = deJargonizer.GetCommonWords(words);

			Assert.Contains(expectedWord, result);
		}

		[Theory]
		[InlineData("banner", "result", "banner", "rfrf", "to")]
		[InlineData("analyze", "to", "and", "analyze", "rfrf", "WEfewgt")]
		[InlineData("scout", "scout")]
		public void GetCommonWords_ShouldReturnOnlyNormalWord_WhenGettingMultipleWords(string expectedWord, params string[] words)
		{
			var result = deJargonizer.GetNormalWords(words);

			Assert.Contains(expectedWord, result);
		}

		[Theory]
		[InlineData("rfrf", "scout", "banner", "rfrf", "to")]
		[InlineData("pare", "analyze", "and", "wegfewwf", "pare", "WEfewgt")]
		[InlineData("carley", "carley")]
		public void GetCommonWords_ShouldReturnOnlyRareWord_WhenGettingMultipleWords(string expectedWord, params string[] words)
		{
			var result = deJargonizer.GetRareWords(words);

			Assert.Contains(expectedWord, result);
		}

		[Fact]
		public void Analyze_ShouldReturnScoreZero_WhenEmptyList()
		{
			var result = deJargonizer.Analyze(new List<string>());

			Assert.Equal(0, result.Score);
		}

		[Theory]
		[InlineData("wefwrfef")]
		[InlineData("carley")]
		[InlineData("pare")]
		public void Analyze_ShouldReturnScoreZero_WhenOneRareWord(string word)
		{
			var result = deJargonizer.Analyze(new List<string> { word });

			Assert.Equal(0, result.Score);
		}

		[Theory]
		[InlineData("analyze")]
		[InlineData("banner")]
		[InlineData("scout")]
		public void Analyze_ShouldReturnScoreFifty_WhenOneNormalWord(string word)
		{
			var result = deJargonizer.Analyze(new List<string> { word });

			Assert.Equal(50, result.Score);
		}

		[Theory]
		[InlineData("to")]
		[InlineData("and")]
		[InlineData("result")]
		public void Analyze_ShouldReturnScoreHundred_WhenOneCommonWord(string word)
		{
			var result = deJargonizer.Analyze(new List<string> { word });

			Assert.Equal(100, result.Score);
		}
	}
}
