using System.Collections.Generic;
using Crawler.PartOfSpeechTagger;

namespace Crawler.Analyzers.UtilAnalyzers
{
	public interface ISentencesAnalyzer
	{
		float CalculatePassiveVoiceSentencesPercentage(List<PosTagToken> posTagTokens);
	}
}