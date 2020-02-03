using System.Collections.Generic;
using Crawler.LexicalAnalyzer;

namespace Crawler.Analyzers
{
	public interface IPunctuationAnalyzer
	{
		double CalculateAverageWordsCountBetweenPunctuation(List<Token> tokens);
		double CalculateMaxWordsCountBetweenPunctuation(List<Token> tokens);
		double CalculateWordsCountsBetweenPunctuationStandardDeviation(List<Token> tokens);
		int CalculateWordsCountDecile(int decile, List<Token> tokens);
	}
}