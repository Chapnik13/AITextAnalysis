using System.Collections.Generic;

namespace Crawler.DeJargonizer
{
	public interface IDeJargonizer
	{
		DeJargonizerResult Analyze(IEnumerable<string> words);

		IEnumerable<string> GetCommonWords(IEnumerable<string> words);

		IEnumerable<string> GetNormalWords(IEnumerable<string> words);

		IEnumerable<string> GetRareWords(IEnumerable<string> words);
	}
}
