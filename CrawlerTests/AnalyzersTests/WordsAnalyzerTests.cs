using Crawler.Analyzers.UtilAnalyzers;
using Crawler.Configs;
using Crawler.DeJargonizer;
using Crawler.Exceptions;
using Crawler.LexicalAnalyzer;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Crawler;
using Crawler.PartOfSpeechTagger;
using Xunit;

namespace CrawlerTests
{
    public class WordsAnalyzerTests
    {
        private readonly WordsAnalyzer wordsAnalyzer;
        private readonly IDataFileLoader dataFileLoader;

        private IOptions<DataFilesConfig> dataFilesConfig;

        public WordsAnalyzerTests()
        {
	        dataFileLoader = Mock.Of<IDataFileLoader>();
	        dataFilesConfig = Mock.Of<IOptions<DataFilesConfig>>();

	        Mock.Get(dataFilesConfig)
		        .Setup(config => config.Value)
		        .Returns(new DataFilesConfig
		        {
			        EmotionsFile = string.Empty,
			        QuestionsFile = string.Empty,
			        NumbersFile = string.Empty
		        });

            wordsAnalyzer = new WordsAnalyzer(
		        Mock.Of<IDeJargonizer>(),
		        Mock.Of<IPosTagger>(),
		        dataFilesConfig,
		        dataFileLoader);
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
	        Mock.Get(dataFileLoader)
		        .Setup(loader => loader.Load(It.IsAny<string>()))
		        .Returns(new List<string>{"one", "four", "three"});


            var result = wordsAnalyzer.CalculateNumbersAsWords(words.Select(w => new Token(eTokenType.StringValue, w)).ToList());
            
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1, "why", "to")]
        [InlineData(3, "where", "what", "whom", "hjh")]
        public void CalculateQuestionWords_ShouldReturnNumberOfAppearences(double expectedResult, params string[] words)
        {
	        Mock.Get(dataFileLoader)
		        .Setup(loader => loader.Load(It.IsAny<string>()))
		        .Returns(new List<string> { "why", "where", "what", "whom" });

            var result = wordsAnalyzer.CalculateQuestionWords(words.Select(w => new Token(eTokenType.StringValue, w)).ToList());

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(0.5, "Unbelievable", "cse")]
        [InlineData(0.25, "Censored", "asd", "ss", "hh")]
        public void CalculateEmotionWords_ShouldReturnNumberOfAppearences(double expectedResult, params string[] words)
        {
	        Mock.Get(dataFileLoader)
		        .Setup(loader => loader.Load(It.IsAny<string>()))
		        .Returns(new List<string> { "unbelievable", "censored" });

            var result = (double)wordsAnalyzer.CalculateEmotionWordsPercentage(words.Select(w => new Token(eTokenType.StringValue, w)).ToList());

            Assert.Equal(expectedResult, result);
        }
    }
}