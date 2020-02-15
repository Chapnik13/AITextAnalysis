using Crawler.Configs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace Crawler.LexicalAnalyzer
{
    public class Lexer : ILexer
    {
        private readonly LexerConfig config;
        private readonly ILogger logger;

        public Lexer(ILogger<Lexer> logger, IOptions<LexerConfig> config)
        {
            this.config = config.Value;
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
                    yield return CleanApostrophe(token);
                }

                text = text.Substring(token?.Value.Length ?? 1);
            }
        }

        private Token? FindToken(string text)
        {
            return config.TokensDefinitions
                .Select(tokenDefinition => tokenDefinition.Match(text))
                .FirstOrDefault(token => token != null);
        }

        private Token CleanApostrophe(Token token)
        {
            var indexOfApostophe = token.Value.IndexOf('\'');

            if (indexOfApostophe == -1) return token;

            var value = token.Value.Substring(0, indexOfApostophe);
            return new Token(token.TokenType, value);
        }
    }
}