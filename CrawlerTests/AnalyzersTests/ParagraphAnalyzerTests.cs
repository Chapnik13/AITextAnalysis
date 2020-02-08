using Crawler.Analyzers;
using Crawler.LexicalAnalyzer;
using System.Collections.Generic;
using Xunit;

namespace CrawlerTests
{
    public class ParagraphAnalyzerTests
    {
        private const float EXPECTED_AVERAGE_LENGTH = 1.5f;
        private const float EXPECTED_AVERAGE_AMOUNT_OF_COMMAS_AND_PERIODS = 1.5f;
        private const int EXPECTED_AVERAGE_AMOUNT_OF_SENTENCES = 1;

        private readonly IParagraphAnalyzer paragraphAnalyzer;

        private readonly List<Token> paragraph1 = new List<Token> { new Token(eTokenType.StringValue, "Hello"), new Token(eTokenType.Punctuation, ","), new Token(eTokenType.StringValue, "World"), new Token(eTokenType.Punctuation, ".") };
        private readonly List<Token> paragraph2 = new List<Token> { new Token(eTokenType.StringValue, "Hello"), new Token(eTokenType.Punctuation, ".") };
        private readonly List<List<Token>> paragraphs;

        public ParagraphAnalyzerTests()
        {
            paragraphAnalyzer = new ParagraphAnalyzer();
            paragraphs = new List<List<Token>> { paragraph1, paragraph2 };
        }

        [Fact]
        public void CalculateAverageLength_SholudReturnOneAndHalf_GivenParagraphs()
        {
            var result = paragraphAnalyzer.CalculateAverageLength(paragraphs);

            Assert.Equal(EXPECTED_AVERAGE_LENGTH, result);
        }

        [Fact]
        public void CalculateAverageAmountOfCommaAndPeriod_SholudReturnOneAndHalf_GivenParagraphs()
        {
            var result = paragraphAnalyzer.CalculateAverageAmountOfCommaAndPeriod(paragraphs);

            Assert.Equal(EXPECTED_AVERAGE_AMOUNT_OF_COMMAS_AND_PERIODS, result);
        }

        [Fact]
        public void CalculateAverageAmountOfSentences_SholudReturnParagraphsLengthAverageOne_GivenParagraphs()
        {
            var result = paragraphAnalyzer.CalculateAverageAmountOfSentences(paragraphs);

            Assert.Equal(EXPECTED_AVERAGE_AMOUNT_OF_SENTENCES, result);
        }
    }
}
