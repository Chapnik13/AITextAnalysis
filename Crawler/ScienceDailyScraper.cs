using Crawler.MockupWrappers;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Crawler
{
    public class ScienceDailyScraper : IScienceDailyScraper
    {
        private const string SELECTOR = "div#text, p#first";

        private readonly IBrowsingContextWrapper context;
        private readonly ILogger logger;

        public ScienceDailyScraper(IBrowsingContextWrapper context, ILogger<ScienceDailyScraper> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<string> ScrapAsync(string address, CancellationToken cancellationToken)
        {
            var document = await context.OpenAsync(address, cancellationToken).ConfigureAwait(false);
            logger.LogDebug("Opened document from {address}", address);

            var nodes = document.QuerySelectorAll(SELECTOR).ToList();

            if (nodes.Count == 0)
            {
                logger.LogError("Could not find {selector} in {address}", SELECTOR, address);

                return string.Empty;
            }

            logger.LogDebug("Extracted {selector} from document", SELECTOR);

            var text = GetText(nodes);

            logger.LogInformation("Extracted text from {address} {selector}", address, SELECTOR);

            return text;
        }

        private string GetText(IEnumerable<INodeWrapper> nodes)
        {
            return nodes.Select(n => n.Text()).Aggregate((x, y) => x + y);
        }
    }
}
