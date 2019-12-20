using Crawler.LexicalAnalyzer;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Crawler
{
    public class CrawlerService : BackgroundService
    {
        private readonly IScienceDailyScraper scienceDailyScraper;
        private readonly ILexer lexer;

        public CrawlerService(IScienceDailyScraper scienceDailyScraper, ILexer lexer)
        {
            this.scienceDailyScraper = scienceDailyScraper;
            this.lexer = lexer;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Enter url");
            var url = Console.ReadLine();

            var text = await scienceDailyScraper.ScrapAsync(url, cancellationToken)
                .ConfigureAwait(false);

            var tokens = lexer.GetTokens(text);

            foreach (var token in tokens)
            {
                Console.Write(token.Value + " ");
            }
        }
    }
}