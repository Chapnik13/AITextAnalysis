using System;

namespace Crawler.Analyzers.AnalysisResults
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AnalysisResultAttribute : Attribute
    {
        public string ArticlePart { get; }

        public AnalysisResultAttribute(string articlePart)
        {
            this.ArticlePart = articlePart;
        }
    }
}
