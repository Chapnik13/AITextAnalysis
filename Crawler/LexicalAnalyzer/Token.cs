using System;

namespace Crawler.LexicalAnalyzer
{
    public class Token
    {
        public eTokenType TokenType { get; }
        public string Value { get; }

        public Token(eTokenType tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }

        public override bool Equals(object? other)
        {
            return other is Token otherToken &&
                   (TokenType == otherToken.TokenType && Value == otherToken.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TokenType, Value);
        }
    }
}
