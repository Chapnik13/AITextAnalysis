using Crawler.LexicalAnalyzer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Crawler.ExtensionMethods
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<string> GetValuesByTokenType(this IEnumerable<Token> tokens, eTokenType tokenType)
        {
            return tokens.Where(t => t.TokenType == tokenType).Select(t => t.Value);
        }

        public static float CalculateAverageOfParagraphs(this IEnumerable<IEnumerable<Token>> paragraphs, Func<Token, bool> predicate)
        {
            return (float)paragraphs.Average(p => p.Count(predicate));
        }
    }
}
