using System.Collections.Generic;

namespace Crawler.DeJargonizer
{
	public interface IWordsCountLoader
	{
		Dictionary<string, int> Load();
	}
}
