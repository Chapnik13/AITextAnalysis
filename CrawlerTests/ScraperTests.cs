using Crawler.Configs;
using Crawler.MockupWrappers;
using Crawler.SiteScraper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CrawlerTests
{
    public class ScraperTests
    {
        private const string A_TEXT = "Hello";
        private const string PATTERN = "sciencedaily";

        private readonly INodeWrapper node;
        private readonly List<INodeWrapper> nodes;
        private readonly IScraper scraper;
        private readonly ScrapersConfig config;

        public ScraperTests()
        {
            var context = Mock.Of<IBrowsingContextWrapper>();
            var document = Mock.Of<IDocumentWrapper>();
            var configOptions = Mock.Of<IOptions<ScrapersConfig>>();

            config = new ScrapersConfig { ScrapesrDefinitions = new[]
            {
                new ScraperDefinition{Pattern = PATTERN, Selector = "div#text"}
            } };

            Mock.Get(configOptions).Setup(c => c.Value).Returns(config);

            node = Mock.Of<INodeWrapper>();
            nodes = new List<INodeWrapper> { node };

            Mock.Get(context)
                .Setup(c => c.OpenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(document));

            Mock.Get(document)
                .Setup(d => d.QuerySelectorAll(It.IsAny<string>()))
                .Returns(nodes);

            scraper = new Scraper(context, Mock.Of<ILogger<Scraper>>(), configOptions);
        }

        [Fact]
        public async Task ScrapAsync_ShouldReturnText_WhenTextIsFound()
        {
            Mock.Get(node).Setup(n => n.Text()).Returns(A_TEXT);

            var result = await RunScrapAsync().ConfigureAwait(false);

            Assert.Contains(result, s => s == A_TEXT);
        }

        [Fact]
        public async Task ScrapAsync_ShouldReturnEmptyText_WhenSelectorNotFound()
        {
            nodes.Clear();

            var result = await RunScrapAsync().ConfigureAwait(false);

            Assert.Empty(result);
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

            Assert.Contains(result, string.IsNullOrWhiteSpace);
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

            Assert.Collection(result, item => Assert.Contains(A_TEXT, item),
                                                        item => Assert.Contains(A_TEXT, item));
        }

        private async Task<IEnumerable<string>> RunScrapAsync() =>
            await scraper.ScrapAsync(PATTERN, It.IsAny<CancellationToken>()).ConfigureAwait(false);

        private string CombineEnumerableToText(IEnumerable<string> t)
        {
            return t.Aggregate((t1, t2) => t1 + t2);
        }
    }
}