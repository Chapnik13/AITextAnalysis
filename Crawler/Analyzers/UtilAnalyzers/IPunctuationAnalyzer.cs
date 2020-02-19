using System.Collections.Generic;
using Crawler.LexicalAnalyzer;
using Crawler.PartOfSpeechTagger;

namespace Crawler.Analyzers.UtilAnalyzers
{
	public interface IPunctuationAnalyzer
	{
		double CalculateAverageWordsCountBetweenPunctuation(List<Token> tokens);

		int CalculateMaxWordsCountBetweenPunctuation(List<Token> tokens);

		double CalculateWordsCountBetweenPunctuationStandardDeviation(List<Token> tokens);

		int CalculateWordsCountDecile(int decile, List<Token> tokens);

		int CountCharacter(char chr, List<Token> tokens);

		double CalculatePassiveVoiceSentencesPercentage(List<PosTagToken> posTagTokens);
	}
}