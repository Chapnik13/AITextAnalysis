using Crawler.LexicalAnalyzer;
using System.Collections.Generic;

namespace Crawler.Analyzers
{
    public interface IParagraphAnalyzer
    {
        float CalculateAverageAmountOfCommaAndPeriod(IEnumerable<IEnumerable<Token>> paragraphs);
        float CalculateAverageAmountOfSentences(IEnumerable<IEnumerable<Token>> paragraphs);
        float CalculateAverageLength(IEnumerable<IEnumerable<Token>> paragraphs);
    }
}