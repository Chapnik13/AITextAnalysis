namespace Crawler.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string RemoveApostrophe(this string word)
        {
            var indexOfApostrophe = word.IndexOf('\'');

            return indexOfApostrophe == -1 ? word : word.Substring(0, indexOfApostrophe);
        }
    }
}
