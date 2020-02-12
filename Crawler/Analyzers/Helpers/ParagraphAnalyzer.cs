using System.Collections.Generic;
using Crawler.ExtensionMethods;
using Crawler.LexicalAnalyzer;

namespace Crawler.Analyzers.Helpers
{
    public class ParagraphAnalyzer : IParagraphAnalyzer
    {
        public float CalculateAverageLength(List<List<Token>> paragraphs)
        {
            return paragraphs.CalculateAverageOfTokenGroups(t => t.TokenType != eTokenType.Punctuation);
        }

        public float CalculateAverageAmountOfCommasAndPeriods(List<List<Token>> paragraphs)
        {
            return paragraphs.CalculateAverageOfTokenGroups(t => t.Value == "." || t.Value == ",");
        }

        public float CalculateAverageAmountOfSentences(List<List<Token>> paragraphs)
        {
            return paragraphs.CalculateAverageOfTokenGroups(t => t.Value == ".");
        }
    }
}
