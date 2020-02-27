using Crawler.LexicalAnalyzer;
using System.Collections.Generic;

namespace Crawler.Analyzers.UtilAnalyzers
{
	public interface IPunctuationAnalyzer
	{
		double CalculateAverageWordsCountBetweenPunctuation(List<Token> tokens);

		int CalculateMaxWordsCountBetweenPunctuation(List<Token> tokens);

		double CalculateWordsCountBetweenPunctuationStandardDeviation(List<Token> tokens);

		int CalculateWordsCountDecile(int decile, List<Token> tokens);

		int CountCharacter(char chr, List<Token> tokens);
	}
}