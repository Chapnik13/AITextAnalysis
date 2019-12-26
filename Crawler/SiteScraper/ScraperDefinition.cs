using System.Text.RegularExpressions;

namespace Crawler.SiteScraper
{
    public class ScraperDefinition
    {
        private Regex regex = new Regex(string.Empty);

        public string Selector { get; set; }

        public string Pattern
        {
            get => regex.ToString();
            set => regex = new Regex(value);
        }

        public string? Match(string url)
        {
            var match = regex.Match(url);

            return match.Success ? Selector : null;
        }
    }
}
