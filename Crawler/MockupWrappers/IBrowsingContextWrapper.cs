using System.Threading;
using System.Threading.Tasks;

namespace Crawler.MockupWrappers
{
    public interface IBrowsingContextWrapper
    {
        Task<IDocumentWrapper> OpenAsync(string address, CancellationToken cts);
    }
}
