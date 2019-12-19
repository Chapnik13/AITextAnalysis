using AngleSharp.Dom;

namespace Crawler.MockupWrappers
{
    public class NodeWrapper : INodeWrapper
    {
        private readonly INode node;

        public NodeWrapper(INode node)
        {
            this.node = node;
        }
        public string Text()
        {
            return node.Text();
        }
    }
}
