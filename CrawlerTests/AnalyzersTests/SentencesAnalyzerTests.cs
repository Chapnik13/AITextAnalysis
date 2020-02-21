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
    public class SentencesAnalyzerTests
    {
	    private const string PASSIVE_VOICE_SENTENCE = "The house will be cleaned by me every Saturday.";
	    private const string ACTIVE_VOICE_SENTENCE = " Sue changed the flat tire. ";

	    private ILexer lexer;
        private LexerConfig config;
        private SentencesAnalyzer sentencesAnalyzer;

        public SentencesAnalyzerTests()
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

	        sentencesAnalyzer = new SentencesAnalyzer(dataFilesConfigOptions);
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
        public void CalculatePassiveVoiceSentencesPercentage_ShouldReturnZero_WhenEmptyList()
        {
	        var result = sentencesAnalyzer.CalculatePassiveVoiceSentencesPercentage(new List<PosTagToken>());

	        Assert.Equal(0, result);
        }

        [Fact]
        public void CalculatePassiveVoiceSentencesPercentage_ShouldReturnHundredPercentage_WhenOnePassiveSentence()
        {
	        var result = sentencesAnalyzer.CalculatePassiveVoiceSentencesPercentage(new List<PosTagToken>
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

	        var result = sentencesAnalyzer.CalculatePassiveVoiceSentencesPercentage(posTagTokens);

	        Assert.Equal(expectedResult, result);
        }
    }
}