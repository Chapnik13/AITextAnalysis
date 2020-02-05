using Crawler.LexicalAnalyzer;
using Crawler.Models;
using System.Collections.Generic;

namespace Crawler.Analyzers
{
    public interface IAnalyzer<out TResult>
    {
        TResult Analyze(Article<List<Token>> article);
    }
}