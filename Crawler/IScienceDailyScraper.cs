using System.Threading;
using System.Threading.Tasks;

namespace Crawler
{
    public interface IScienceDailyScraper
    {
        Task<string> ScrapAsync(string address, CancellationToken cancellationToken);
    }
}
