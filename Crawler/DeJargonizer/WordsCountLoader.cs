using Crawler.Configs;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper.Configuration;
using Microsoft.Extensions.Options;

namespace Crawler.DeJargonizer
{
	public class WordsCountLoader : IWordsCountLoader
	{
		private readonly string wordsCountMatrixPath;

		public WordsCountLoader(IOptions<DataFilesConfig> dataFilesConfig)
		{
			wordsCountMatrixPath = dataFilesConfig.Value.WordsCountMatrix;
		}

		public Dictionary<string, int> Load()
		{
			using var reader = new StreamReader(new FileStream(wordsCountMatrixPath, FileMode.Open, FileAccess.Read));
            using var csv = new CsvReader(reader,
                new CsvConfiguration(CultureInfo.CurrentCulture) {HasHeaderRecord = false});

            return csv.GetRecords<WordCount>().ToDictionary(p => p.Word, p => p.Count);
		}
	}
}
