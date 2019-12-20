using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Crawler
{
    public class CrawlerService : BackgroundService
    {
        private readonly IScienceDailyScraper scienceDailyScraper;

        public CrawlerService(IScienceDailyScraper scienceDailyScraper)
        {
            this.scienceDailyScraper = scienceDailyScraper;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var text = await scienceDailyScraper.ScrapAsync("https://www.sciencedaily.com/releases/2019/12/191213143307.htm", cancellationToken).ConfigureAwait(false);
        }
    }
}