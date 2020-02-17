using System.Collections.Generic;
using Crawler.LexicalAnalyzer;

namespace Crawler.PartOfSpeechTagger
{
	public interface IPosTagger
	{
		List<PosTagToken> Tag(List<Token> tokens);
	}
}