using System.Collections.Generic;

namespace Crawler.Models
{
    public class Article<T>
    {
        public T Title { get; set; }
        public T Subtitle { get; set; }
        public List<T> Paragraphs { get; set; }
    }
}
