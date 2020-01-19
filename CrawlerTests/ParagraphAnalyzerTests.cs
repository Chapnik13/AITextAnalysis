using Crawler.Analyzers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CrawlerTests
{
    public class ParagraphAnalyzerTests
    {
        private readonly IParagraphAnalyzer paragraphAnalyzer;

        public ParagraphAnalyzerTests()
        {
            paragraphAnalyzer = new ParagraphAnalyzer();
        }

        [Fact]
        public void ddd()
        {
            var result = paragraphAnalyzer.CalculateAverageLength()
        }
    }
}
