using System.Collections.Generic;

namespace Crawler.MockupWrappers
{
    public interface IDocumentWrapper
    {
        IEnumerable<INodeWrapper> QuerySelectorAll(string selector);
    }
}