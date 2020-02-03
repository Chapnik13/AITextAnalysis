using Crawler.Configs;
using Crawler.Exceptions;
using Crawler.MockupWrappers;
using Crawler.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

        public async Task<Article<string>?> ScrapAsync(string address, CancellationToken cancellationToken)
        {
            var document = await context.OpenAsync(address, cancellationToken).ConfigureAwait(false);
            logger.LogDebug("Opened document from {address}", address);

            var selector = FindSelector(address);

            if (selector is null)
            {
                logger.LogError("Could not find selector from config for {address}", address);
                throw new SelectorNotFoundException($"Selector for site {address} not found");
            }

            var article = new Article<string>
            {
                Title = document.QuerySelectorAll(selector.TitleSelector).Select(n => n.Text()).First(),
                Subtitle = document.QuerySelectorAll(selector.SubtitleSelector).Select(n => n.Text()).First(),
                Paragraphs = document.QuerySelectorAll(selector.TextSelector).Select(n => n.Text()).ToList()
            };

            if (article.Title is null || article.Subtitle is null || !article.Paragraphs.Any())
            {
                logger.LogError("Could not find {@selector} in {address}", selector, address);

                return null;
            }

            logger.LogInformation("Extracted paragraphs from {address} {@selector}", address, selector);

            return article;
        }

        private ScraperDefinition FindSelector(string url)
        {
            return config.ScrapesrDefinitions
                .FirstOrDefault(scraperDefinition => scraperDefinition.Match(url));
        }
    }
}
