using AngleSharp.Dom;
using System.Collections.Generic;
using System.Linq;

namespace Crawler.MockupWrappers
{
    public class DocumentWrapper : IDocumentWrapper
    {
        private readonly IDocument document;
        public DocumentWrapper(IDocument document)
        {
            this.document = document;
        }

        public IEnumerable<INodeWrapper> QuerySelectorAll(string selector)
        {
            var nodes = document.QuerySelectorAll(selector);

            return nodes.Select(n => (INodeWrapper)new NodeWrapper(n));
        }
    }
}