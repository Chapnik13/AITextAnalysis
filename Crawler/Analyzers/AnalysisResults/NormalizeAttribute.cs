using System;

namespace Crawler.Analyzers.AnalysisResults
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NormalizeAttribute : Attribute
    {
    }
}