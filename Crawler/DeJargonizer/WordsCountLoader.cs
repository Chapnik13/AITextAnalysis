using Crawler.Configs;
using CsvHelper;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;

namespace Crawler.DeJargonizer
{
	public class WordsCountLoader : IWordsCountLoader
	{
		private string wordsCountMatrixPath;

		public WordsCountLoader(IOptions<WordsCountMatrixConfig> wordsCountMatrixConfig)
		{
			wordsCountMatrixPath = wordsCountMatrixConfig.Value.Path;
		}

		public Dictionary<string, int> Load()
		{
			using var reader = new StreamReader(new FileStream(wordsCountMatrixPath, FileMode.Open, FileAccess.Read));
			using var csv = new CsvReader(reader);
			csv.Configuration.HasHeaderRecord = false;

			return csv.GetRecords<WordCount>().ToDictionary(p => p.Word, p => p.Count);
		}
	}
}
