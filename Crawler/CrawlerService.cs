using Crawler.LexicalAnalyzer;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crawler.Configs;
using Crawler.DeJargonizer;
using Microsoft.Extensions.Options;

namespace Crawler
{
    public class CrawlerService : BackgroundService
    {
        private readonly IScienceDailyScraper scienceDailyScraper;
        private readonly ILexer lexer;
        private readonly IHostApplicationLifetime applicationLifetime;
        private readonly DeJargonizer.DeJargonizer deJargonizer;

        public CrawlerService(
	        IScienceDailyScraper scienceDailyScraper,
	        ILexer lexer,
	        IHostApplicationLifetime applicationLifetime,
	        IWordsCountLoader wordsCountLoader,
	        IOptions<WordsCountThresholdsConfig> wordsCountThresholdsConfig)
        {
            this.scienceDailyScraper = scienceDailyScraper;
            this.lexer = lexer;
            this.applicationLifetime = applicationLifetime;
            deJargonizer = new DeJargonizer.DeJargonizer(wordsCountLoader, wordsCountThresholdsConfig.Value);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.Write("Enter url ");
            var url = Console.ReadLine();

            var text = await scienceDailyScraper.ScrapAsync(url, cancellationToken)
                .ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine($"No text was found at {url}");
            }

            var tokens = lexer.GetTokens(text);

            var score = deJargonizer.Analyze(tokens.Where(t => t.TokenType == eTokenType.StringValue).Select(t => t.Value)).Score;

            Console.WriteLine(text);
            Console.WriteLine($"Score: {score}");

			applicationLifetime.StopApplication();
        }
    }
}