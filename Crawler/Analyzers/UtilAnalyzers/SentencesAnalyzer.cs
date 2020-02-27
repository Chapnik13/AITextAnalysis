using Crawler.Configs;
using Crawler.PartOfSpeechTagger;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace Crawler.Analyzers.UtilAnalyzers
{
	public class SentencesAnalyzer : ISentencesAnalyzer
	{
		private readonly IOptions<DataFilesConfig> config;
		private readonly IDataFileLoader dataFileLoader;

		public SentencesAnalyzer(IOptions<DataFilesConfig> dataFilesConfig, IDataFileLoader dataFileLoader)
		{
			this.config = dataFilesConfig;
			this.dataFileLoader = dataFileLoader;
		}

		public float CalculatePassiveVoiceSentencesPercentage(List<PosTagToken> posTagTokens)
		{
			var passiveVoiceSentencesCount = 0;
			var indexOfNextSentenceEnd = posTagTokens.FindIndex(token => token.ExtendedType == ePosTagExtendedType.EndOfSentence);
			var totalSentencesCount = posTagTokens.Count(token => token.ExtendedType == ePosTagExtendedType.EndOfSentence);

			if (totalSentencesCount == 0)
			{
				totalSentencesCount = 1;
			}

			while (indexOfNextSentenceEnd != -1)
			{
				var sentence = posTagTokens.Take(indexOfNextSentenceEnd).ToList();

				if (IsPassiveVoiceSentence(sentence))
				{
					passiveVoiceSentencesCount++;
				}

				posTagTokens = posTagTokens.Skip(indexOfNextSentenceEnd + 1).ToList();
				indexOfNextSentenceEnd = posTagTokens.FindIndex(token => token.ExtendedType == ePosTagExtendedType.EndOfSentence);
			}

			return passiveVoiceSentencesCount / (float)totalSentencesCount;
		}

		private bool IsPassiveVoiceSentence(List<PosTagToken> sentence)
		{
			var toBeForms = dataFileLoader.Load(config.Value.ToBeFormsFile);
				
			var indexOfToBeForm = sentence.FindIndex(token => toBeForms.Contains(token.Value.ToLower()));
			var indexOfPastParticiple = sentence.FindIndex(token => token.ExtendedType == ePosTagExtendedType.PastParticipleVerb);

			var isPassiveVoiceSentence = indexOfToBeForm != -1 && indexOfPastParticiple != -1 && indexOfToBeForm < indexOfPastParticiple;

			return isPassiveVoiceSentence;
		}
	}
}
