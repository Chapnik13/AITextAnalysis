using System;
using System.Collections.Generic;
using System.Linq;

namespace Crawler.DeJargonizer
{
	public class DeJargonizerResult
	{
		public IEnumerable<string> AllWords { get; }

		public IEnumerable<string> CommonWords { get; }

		public IEnumerable<string> NormalWords { get; }

		public IEnumerable<string> RareWords { get; }

		public int Score { get; }

		internal DeJargonizerResult(
			IEnumerable<string> allWords,
			IEnumerable<string> commonWords,
			IEnumerable<string> normalWords,
			IEnumerable<string> rareWords)
		{
			CommonWords = commonWords;
			NormalWords = normalWords;
			RareWords = rareWords;
			AllWords = allWords.ToList();

			Score = !AllWords.Any() ? 0 :
				(int)Math.Round(100 - (NormalWords.Count() * 0.5f + RareWords.Count()) * 100 / AllWords.Count());
		}
	}
}
