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
    }
}
