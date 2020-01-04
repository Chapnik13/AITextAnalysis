using Crawler.Configs;
using Crawler.MockupWrappers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crawler.Exceptions;

namespace Crawler.SiteScraper
{
    public class Scraper : IScraper
    {
        private readonly IBrowsingContextWrapper context;
        private readonly ILogger logger;
        private readonly ScrapersConfig config;

        public Scraper(IBrowsingContextWrapper context, ILogger<Scraper> logger, IOptions<ScrapersConfig> config)
        {
            this.context = context;
            this.logger = logger;
            this.config = config.Value;
        }

        public async Task<string> ScrapAsync(string address, CancellationToken cancellationToken)
        {
            var document = await context.OpenAsync(address, cancellationToken).ConfigureAwait(false);
            logger.LogDebug("Opened document from {address}", address);

            var selector = FindSelector(address);

            if (selector is null)
            {
                logger.LogError("Could not find selector from config for {address}", address);
                throw new SelectorNotFoundException($"Selector for site {address} not found");
            }

            var nodes = document.QuerySelectorAll(selector).ToList();

            if (!nodes.Any())
            {
                logger.LogError("Could not find {selector} in {address}", selector, address);

                return string.Empty;
            }

            logger.LogDebug("Extracted {selector} from document", selector);

            var text = GetText(nodes);

            logger.LogInformation("Extracted text from {address} {selector}", address, selector);

            return text;
        }

        private string? FindSelector(string url)
        {
            return config.ScrapesrDefinitions
                .Select(scraperDefinition => scraperDefinition.Match(url))
                .FirstOrDefault(selector => selector != null);
        }

        private string GetText(IEnumerable<INodeWrapper> nodes)
        {
            return nodes.Select(n => n.Text()).Aggregate((x, y) => x + y);
        }
    }
}
