using System.Collections.Generic;
using Crawler.ExtensionMethods;
using Crawler.LexicalAnalyzer;

namespace Crawler.Analyzers.UtilAnalyzers
{
    public class ParagraphsAnalyzer : IParagraphsAnalyzer
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
