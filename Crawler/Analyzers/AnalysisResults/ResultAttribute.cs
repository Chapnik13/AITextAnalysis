using System;

namespace Crawler.Analyzers.AnalysisResults
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ResultAttribute : Attribute
    {
        public string Format { get; }

        public ResultAttribute(string format)
        {
            this.Format = format;
        }
    }
}
