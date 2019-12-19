using Crawler;
using Crawler.Exceptions;
using Crawler.MockupWrappers;
using Microsoft.Extensions.Logging;
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

        private readonly INodeWrapper node;
        private readonly List<INodeWrapper> nodes;
        private readonly ScienceDailyScraper scraper;

        public ScienceDailyScraperTests()
        {
            var context = Mock.Of<IBrowsingContextWrapper>();
            var document = Mock.Of<IDocumentWrapper>();

            node = Mock.Of<INodeWrapper>();
            nodes = new List<INodeWrapper> { node };

            Mock.Get(context)
                .Setup(c => c.OpenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(document));

            Mock.Get(document)
                .Setup(d => d.QuerySelectorAll(It.IsAny<string>()))
                .Returns(nodes);

            scraper = new ScienceDailyScraper(context, Mock.Of<ILogger<ScienceDailyScraper>>());
        }

        [Fact]
        public async Task ScrapAsync_ShouldReturnText_WhenTextIsFound()
        {
            Mock.Get(node).Setup(n => n.Text()).Returns(A_TEXT);

            var result = await RunScrapAsync().ConfigureAwait(false);

            Assert.Equal(A_TEXT, result);
        }

        [Fact]
        public async Task ScrapAsync_ShouldThrowAnException_WhenSelectorNotFound()
        {
            nodes.Clear();

            await Assert.ThrowsAsync<ScrapFailedException>(RunScrapAsync)
                .ConfigureAwait(false);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public async Task ScrapAsync_ShouldThrowAnException_WhenHtmlNodeTextIsEmpty(string htmlNodeText)
        {
            Mock.Get(node).Setup(n => n.Text()).Returns(htmlNodeText);

            await Assert.ThrowsAsync<ScrapFailedException>(RunScrapAsync)
                .ConfigureAwait(false);
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
            await scraper.ScrapAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()).ConfigureAwait(false);
    }
}