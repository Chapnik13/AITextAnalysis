using Crawler.LexicalAnalyzer;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Crawler
{
    public class CrawlerService : BackgroundService
    {
        private readonly IScienceDailyScraper scienceDailyScraper;
        private readonly ILexer lexer;
        private readonly IHostApplicationLifetime applicationLifetime;

        public CrawlerService(IScienceDailyScraper scienceDailyScraper, ILexer lexer, IHostApplicationLifetime applicationLifetime)
        {
            this.scienceDailyScraper = scienceDailyScraper;
            this.lexer = lexer;
            this.applicationLifetime = applicationLifetime;
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

            tokens.ToList().ForEach(t => Console.Write($"{t.Value} "));

            applicationLifetime.StopApplication();
        }
    }
}