using System;

namespace Crawler.Analyzers.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NormalizeAttribute : Attribute
    {
    }
}