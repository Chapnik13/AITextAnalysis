using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Crawler.SiteScraper
{
    public interface IScraper
    {
        Task<List<string>> ScrapAsync(string address, CancellationToken cancellationToken);
    }
}
