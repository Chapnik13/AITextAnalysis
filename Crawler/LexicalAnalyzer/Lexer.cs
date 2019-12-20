using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Crawler.LexicalAnalyzer
{
    public class Lexer : ILexer
    {
        private readonly List<TokenDefinition> tokenDefinitions;
        private readonly ILogger logger;

        public Lexer(ILogger<Lexer> logger)
        {
            tokenDefinitions = new List<TokenDefinition>
            {
                new TokenDefinition(eTokenType.StringValue, "^[A-Za-z]+"),
                new TokenDefinition(eTokenType.Number, "^[0-9]+"),
                new TokenDefinition(eTokenType.Punctuation, @"^[,\.]")
            };

            this.logger = logger;
        }

        public IEnumerable<Token> GetTokens(string text)
        {
            while (!string.IsNullOrWhiteSpace(text))
            {
                var token = FindToken(text);

                if (token != null)
                {
                    logger.LogDebug("Extracted {@Token}", token);
                    yield return token;
                }

                text = text.Substring(token?.Value.Length ?? 1);
            }
        }

        private Token? FindToken(string text)
        {
            foreach (var tokenDefinition in tokenDefinitions)
            {
                var token = tokenDefinition.Match(text);

                if (token != null)
                {
                    return token;
                }
            }

            return null;
        }
    }
}