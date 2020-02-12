using System;

namespace Crawler.Analyzers.AnalysisResults
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ResultAttribute : Attribute
    {
        public string Description { get; }
        public string Unit { get; }

        public ResultAttribute(string description, string unit)
        {
            this.Description = description;
            this.Unit = unit;
        }

        public ResultAttribute(string description) : this(description, string.Empty)
        {
        }
    }
}
