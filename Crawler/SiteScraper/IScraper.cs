using Crawler.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Crawler.SiteScraper
{
    public interface IScraper
    {
        Task<Article<string>> ScrapAsync(string address, CancellationToken cancellationToken);
    }
}
