using System.Collections.Generic;

namespace Crawler.LexicalAnalyzer
{
    public interface ILexer
    {
        IEnumerable<Token> GetTokens(string text);
    }
}