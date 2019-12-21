using System.Text.RegularExpressions;

namespace Crawler.LexicalAnalyzer
{
    public class TokenDefinition
    {
        private readonly Regex regex;
        private readonly eTokenType tokenType;

        public TokenDefinition(eTokenType tokenType, string regexPattern)
        {
            regex = new Regex(regexPattern);
            this.tokenType = tokenType;
        }

        public Token? Match(string text)
        {
            var match = regex.Match(text);

            return match.Success ? new Token(tokenType, match.Value) : null;
        }
    }
}
