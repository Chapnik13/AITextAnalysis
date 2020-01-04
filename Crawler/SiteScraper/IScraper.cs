using System.Threading;
using System.Threading.Tasks;

namespace Crawler.SiteScraper
{
    public interface IScraper
    {
        Task<string> ScrapAsync(string address, CancellationToken cancellationToken);
    }
}
