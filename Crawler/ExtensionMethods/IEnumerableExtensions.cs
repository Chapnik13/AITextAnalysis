using Crawler.LexicalAnalyzer;
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
	}
}
