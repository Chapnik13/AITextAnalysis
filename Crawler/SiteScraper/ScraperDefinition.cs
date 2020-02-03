using System;
using System.Text.RegularExpressions;

namespace Crawler.SiteScraper
{
    public class ScraperDefinition
    {
        private Regex regex = new Regex(string.Empty);

        public string TitleSelector { get; set; }
        public string SubtitleSelector { get; set; }
        public string TextSelector { get; set; }

        public string UrlPattern
        {
            get => regex.ToString();
            set => regex = new Regex(value);
        }

        public bool Match(string url)
        {
            return regex.Match(url).Success;
        }
    }
}
