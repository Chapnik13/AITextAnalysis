using System.Collections.Generic;
using Crawler.LexicalAnalyzer;

namespace Crawler.Analyzers.Helpers
{
	public interface IPunctuationAnalyzer
	{
		double CalculateAverageWordsCountBetweenPunctuation(List<Token> tokens);

		double CalculateMaxWordsCountBetweenPunctuation(List<Token> tokens);

		double CalculateWordsCountBetweenPunctuationStandardDeviation(List<Token> tokens);

		int CalculateWordsCountDecile(int decile, List<Token> tokens);

		int CountCharacter(char chr, List<Token> tokens);
	}
}