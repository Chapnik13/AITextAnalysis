using Crawler.Configs;
using Crawler.MockupWrappers;
using Crawler.SiteScraper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CrawlerTests
{
    public class ScienceDailyScraperTests
    {
        private const string A_TEXT = "Hello";
        private const string PATTERN = "sciencedaily";

        private readonly INodeWrapper node;
        private readonly List<INodeWrapper> nodes;
        private readonly IScraper scraper;
        private readonly ScrapersConfig config;

        public ScienceDailyScraperTests()
        {
            var context = Mock.Of<IBrowsingContextWrapper>();
            var document = Mock.Of<IDocumentWrapper>();
            var options = Mock.Of<IOptions<ScrapersConfig>>();

            config = new ScrapersConfig { ScrapesrDefinitions = new ScraperDefinition[]
            {
                new ScraperDefinition{Pattern = PATTERN, Selector = "div#text"}
            } };

            Mock.Get(options).Setup(c => c.Value).Returns(config);

            node = Mock.Of<INodeWrapper>();
            nodes = new List<INodeWrapper> { node };

            Mock.Get(context)
                .Setup(c => c.OpenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(document));

            Mock.Get(document)
                .Setup(d => d.QuerySelectorAll(It.IsAny<string>()))
                .Returns(nodes);

            scraper = new Scraper(context, Mock.Of<ILogger<Scraper>>(), options);
        }

        [Fact]
        public async Task ScrapAsync_ShouldReturnText_WhenTextIsFound()
        {
            Mock.Get(node).Setup(n => n.Text()).Returns(A_TEXT);

            var result = await RunScrapAsync().ConfigureAwait(false);

            Assert.Equal(A_TEXT, result);
        }

        [Fact]
        public async Task ScrapAsync_ShouldReturnEmptyText_WhenSelectorNotFound()
        {
            nodes.Clear();

            var result = await RunScrapAsync().ConfigureAwait(false);

            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public async Task ScrapAsync_ShouldReturnEmptyText_WhenHtmlNodeTextIsEmpty(string htmlNodeText)
        {
            Mock.Get(node).Setup(n => n.Text()).Returns(htmlNodeText);

            var result = await RunScrapAsync().ConfigureAwait(false);

            Assert.True(string.IsNullOrWhiteSpace(result));
        }

        [Fact]
        public async Task ScrapAsync_ShouldReturnConcatedText_WhenMultipleHtmlElementsFound()
        {
            nodes.Add(node);
            Mock.Get(node)
                .SetupSequence(n => n.Text())
                .Returns(A_TEXT)
                .Returns(A_TEXT);

            var result = await RunScrapAsync().ConfigureAwait(false);

            Assert.Equal(A_TEXT + A_TEXT, result);
        }

        private async Task<string> RunScrapAsync() =>
            await scraper.ScrapAsync(PATTERN, It.IsAny<CancellationToken>()).ConfigureAwait(false);
    }
}