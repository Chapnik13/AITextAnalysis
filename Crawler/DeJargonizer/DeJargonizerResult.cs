using System;
using System.Collections.Generic;
using System.Linq;

namespace Crawler.DeJargonizer
{
	public class DeJargonizerResult
	{
		public List<string> AllWords { get; }

		public List<string> CommonWords { get; }

		public double CommonWordsPercentage { get; }

		public List<string> NormalWords { get; }

		public double NormalWordsPercentage { get; }

		public List<string> RareWords { get; }

		public double RareWordsPercentage { get; }

		public int Score { get; }

		internal DeJargonizerResult(
			IEnumerable<string> allWords,
			IEnumerable<string> commonWords,
			IEnumerable<string> normalWords,
			IEnumerable<string> rareWords)
		{
			CommonWords = commonWords.ToList();
			NormalWords = normalWords.ToList();
			RareWords = rareWords.ToList();
			AllWords = allWords.ToList();

			CommonWordsPercentage = CommonWords.Count / (double)AllWords.Count;
			NormalWordsPercentage = NormalWords.Count / (double)AllWords.Count;
			RareWordsPercentage = RareWords.Count / (double)AllWords.Count;

			Score = !AllWords.Any() ? 0 :
				(int)Math.Round(100 - (NormalWords.Count * 0.5f + RareWords.Count) * 100 / AllWords.Count);
		}
	}
}
