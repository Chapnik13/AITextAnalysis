using Crawler.ExtensionMethods;
using Crawler.LexicalAnalyzer;
using System.Collections.Generic;

namespace Crawler.Analyzers
{
    public class ParagraphAnalyzer : IParagraphAnalyzer
    {
        public float CalculateAverageLength(IEnumerable<IEnumerable<Token>> paragraphs)
        {
            return paragraphs.CalculateAverageOfTokenGroups(t => t.TokenType != eTokenType.Punctuation);
        }

        public float CalculateAverageAmountOfCommaAndPeriod(IEnumerable<IEnumerable<Token>> paragraphs)
        {
            return paragraphs.CalculateAverageOfTokenGroups(t => t.Value == "." || t.Value == ",");
        }

        public float CalculateAverageAmountOfSentences(IEnumerable<IEnumerable<Token>> paragraphs)
        {
            return paragraphs.CalculateAverageOfTokenGroups(t => t.Value == ".");
        }
    }
}
