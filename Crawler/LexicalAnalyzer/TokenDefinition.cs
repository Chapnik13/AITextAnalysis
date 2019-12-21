using System.Text.RegularExpressions;

namespace Crawler.LexicalAnalyzer
{
    public class TokenDefinition
    {
        private Regex regex = new Regex(string.Empty);

        public eTokenType TokenType { get; set; }

        public string Pattern
        {
            get => regex.ToString();
            set => regex = new Regex(value);
        }

        public Token? Match(string text)
        {
            var match = regex.Match(text);

            return match.Success ? new Token(TokenType, match.Value) : null;
        }
    }
}
