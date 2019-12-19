using AngleSharp;
using System.Threading;
using System.Threading.Tasks;

namespace Crawler.MockupWrappers
{
    public class BrowsingContextWrapper : IBrowsingContextWrapper
    {
        private readonly IBrowsingContext context;
        public BrowsingContextWrapper(IBrowsingContext context)
        {
            this.context = context;
        }

        public async Task<IDocumentWrapper> OpenAsync(string address, CancellationToken cts)
        {
            return new DocumentWrapper(await context.OpenAsync(address, cts).ConfigureAwait(false));
        }
    }
}
