using Crawler.Analyzers.UtilAnalyzers;
using Crawler.Configs;
using Crawler.Exceptions;
using Crawler.LexicalAnalyzer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Crawler.PartOfSpeechTagger;
using Xunit;

namespace CrawlerTests.AnalyzersTests
{
    public class PunctuationAnalyzerTests
    {
	    private const string PASSIVE_VOICE_SENTENCE = "The house will be cleaned by me every Saturday.";
	    private const string ACTIVE_VOICE_SENTENCE = " Sue changed the flat tire. ";

	    private ILexer lexer;
        private LexerConfig config;
        private PunctuationAnalyzer punctuationsAnalyzer;

        public PunctuationAnalyzerTests()
        {
	        InitLexer();
	        InitPunctuationAnalyzer();
        }

        private void InitPunctuationAnalyzer()
        {
	        var dataFilesConfigOptions = Mock.Of<IOptions<DataFilesConfig>>();
	        var dataFilesConfig = new DataFilesConfig
	        {
		        ToBeFormsFile = "data/ToBeForms.csv"
	        };

	        Mock.Get(dataFilesConfigOptions)
		        .Setup(options => options.Value)
		        .Returns(dataFilesConfig);

	        punctuationsAnalyzer = new PunctuationAnalyzer(dataFilesConfigOptions);
        }

        private void InitLexer()
        {
	        var configOptions = Mock.Of<IOptions<LexerConfig>>();

	        config = new LexerConfig
	        {
		        TokensDefinitions = new[]
		        {
			        new TokenDefinition {TokenType = eTokenType.StringValue, Pattern = "^[a-zA-Z]+"},
			        new TokenDefinition {TokenType = eTokenType.Number, Pattern = "^[0-9]+"},
			        new TokenDefinition {TokenType = eTokenType.Punctuation, Pattern = "^[,\\.?!\"-:]"}
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
        [InlineData(1, ",?fcbdrhdt,")]
        [InlineData(3, ",gg bu bg ??,")]
        [InlineData(5, ", segs? sgrsgs ?sgs sg ?hh,")]
        public void CalculateAverageWordCountBetweenPunctuations_ShouldReturnWordsCount_WhenOnePunctuationsPair(
            int expectedResult, string sentence)
        {
            var result = punctuationsAnalyzer.CalculateAverageWordsCountBetweenPunctuation(lexer.GetTokens(sentence).ToList());

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1, ",fcbdrhdt, !sefsfesf,")]
        [InlineData(2, ",gg bu!! bg , fdg.")]
        [InlineData(4, ", segs! sgrsgs sgs !sg !!hh, gn gg gh.")]
        public void CalculateAverageWordCountBetweenPunctuations_ShouldReturnWordsCountAverage_WhenMoreThanOnePunctuationsPair(int expectedResult, string sentence)
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
        [InlineData(5, ", segs!! sgrsgs sgs sg hh,")]
        public void CalculateMaxWordCountBetweenPunctuations_ShouldReturnWordsCount_WhenOnePunctuationsPair(int expectedResult, string sentence)
        {
            var result = punctuationsAnalyzer.CalculateMaxWordsCountBetweenPunctuation(lexer.GetTokens(sentence).ToList());

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(2, ",fcbdrhdt, sefsf esf,")]
        [InlineData(3, ",gg bu bg , fdg.")]
        [InlineData(5, ", segs sgrsgs sgs sg hh, gn gg gh.")]
        public void CalculateMaxWordCountBetweenPunctuations_ShouldReturnMaxWordsCount_WhenMoreThanOnePunctuationsPair(int expectedResult, string sentence)
        {
            var result = punctuationsAnalyzer.CalculateMaxWordsCountBetweenPunctuation(lexer.GetTokens(sentence).ToList());

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void
            CalculateStandardDeviation_ShouldThrowStandardDeviationInvalidArgumentsAmountException_WhenEmptyList()
        {
            Assert.Throws<StandardDeviationInvalidArgumentsAmountException>(
                () => punctuationsAnalyzer.CalculateWordsCountBetweenPunctuationStandardDeviation(new List<Token>())
            );
        }

        [Fact]
        public void CalculateStandardDeviation_ShouldThrowStandardDeviationInvalidArgumentsAmountException_WhenOneWord()
        {
            Assert.Throws<StandardDeviationInvalidArgumentsAmountException>(
                () => punctuationsAnalyzer.CalculateWordsCountBetweenPunctuationStandardDeviation(
                    new List<Token>
                    {
                        new Token(eTokenType.StringValue, It.IsAny<string>())
                    })
            );
        }

        [Theory]
        [InlineData(0, ",efe, cse.")]
        [InlineData(0.5, ", sf, wf regt, rth umu. aede werg,")]
        public void CalculateStandardDeviation_ShouldReturnWordsLengthStandardDeviation_WhenMoreThanOneWords(double expectedResult, string sentence)
        {
            var result = punctuationsAnalyzer.CalculateWordsCountBetweenPunctuationStandardDeviation(lexer.GetTokens(sentence).ToList());

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

        [Fact]
        public void CountCharacter_ShouldReturnZero_WhenEmptyList()
        {
            var result = punctuationsAnalyzer.CountCharacter('-', new List<Token>());

            Assert.Equal(0, result);
        }

        [Fact]
        public void CountCharacter_ShouldReturnOne_WhenOneChar()
        {
            var result = punctuationsAnalyzer.CountCharacter('-', new List<Token> { new Token(eTokenType.Punctuation, "-") });

            Assert.Equal(1, result);
        }

        [Theory]
        [InlineData(2, '-', "esfesf - sefsef sef-s sf")]
        [InlineData(4, '?', "esfesf ???? sefsef sefs sf")]
        [InlineData(7, '!', "e?sfesf! - !!!sefsef! sef!!--s sf?")]
        public void CountCharacter_ShouldReturnCharacterCount_WhenMoreThanOne(int expectedResult, char chr, string sentence)
        {
            var result = punctuationsAnalyzer.CountCharacter(chr, lexer.GetTokens(sentence).ToList());

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CalculatePassiveVoiceSentencesPercentage_ShouldReturnZero_WhenEmptyList()
        {
	        var result = punctuationsAnalyzer.CalculatePassiveVoiceSentencesPercentage(new List<PosTagToken>());

	        Assert.Equal(0, result);
        }

        [Fact]
        public void CalculatePassiveVoiceSentencesPercentage_ShouldReturnHundredPercentage_WhenOnePassiveSentence()
        {
	        var result = punctuationsAnalyzer.CalculatePassiveVoiceSentencesPercentage(new List<PosTagToken>
	        {
		        new PosTagToken{Value = "am"},
                new PosTagToken{ExtendedType = "VBN"},
                new PosTagToken{ExtendedType = "."}
	        });

	        Assert.Equal(1, result);
        }

        [Theory]
        [InlineData(0, new[] { ACTIVE_VOICE_SENTENCE})]
        [InlineData(0.25, new[] { ACTIVE_VOICE_SENTENCE, ACTIVE_VOICE_SENTENCE, ACTIVE_VOICE_SENTENCE, PASSIVE_VOICE_SENTENCE})]
        [InlineData(0.5, new[] { ACTIVE_VOICE_SENTENCE, PASSIVE_VOICE_SENTENCE})]
        [InlineData(0.75, new[] { ACTIVE_VOICE_SENTENCE, PASSIVE_VOICE_SENTENCE, PASSIVE_VOICE_SENTENCE, PASSIVE_VOICE_SENTENCE})]
        [InlineData(1, new[] { PASSIVE_VOICE_SENTENCE })]
        public void CalculatePassiveVoiceSentencesPercentage_ShouldReturnPassiveVoiceSentencesPercentage_WhenMoreThanOne(double expectedResult, string[] sentences)
        {
	        var tokens = lexer.GetTokens(string.Join(' ', sentences)).ToList();
            var posTagTokens = new NodeJSPosTagger(new PosTagTypeClassifier()).Tag(tokens);

	        var result = punctuationsAnalyzer.CalculatePassiveVoiceSentencesPercentage(posTagTokens);

	        Assert.Equal(expectedResult, result);
        }
    }
}