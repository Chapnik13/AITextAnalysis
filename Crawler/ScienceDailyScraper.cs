using Crawler.Exceptions;
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

            var nodes = document.QuerySelectorAll(SELECTOR);

            if (!nodes.Any())
            {
                logger.LogError("Could not find {selector} in {address}", SELECTOR, address);
                throw new ScrapFailedException($"Could not find {SELECTOR} in {address}");
            }

            logger.LogDebug("Extracted {selector} from document", SELECTOR);

            var text = GetText(nodes);

            if (string.IsNullOrWhiteSpace(text))
            {
                logger.LogError("There was no text in {address} in {selector}", address, SELECTOR);
                throw new ScrapFailedException($"There was no text in {address} {SELECTOR}");
            }

            logger.LogDebug("Extracted text from {selector}", SELECTOR);

            logger.LogInformation("Extracted text from {address} {selector}", address, SELECTOR);

            return text;
        }

        private string GetText(IEnumerable<INodeWrapper> nodes)
        {
            return nodes.Select(n => n.Text()).Aggregate((x, y) => x + y);
        }
    }
}
