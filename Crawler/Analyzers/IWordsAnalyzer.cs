using System.Collections.Generic;
using Crawler.DeJargonizer;
using Crawler.LexicalAnalyzer;

namespace Crawler.Analyzers
{
	public interface IWordsAnalyzer
	{
		float CalculateAverageLength(IEnumerable<Token> tokens);

		double CalculateWordsLengthStandardDeviation(IEnumerable<Token> tokens);

		DeJargonizerResult CalculateDeJargonizer(IEnumerable<Token> tokens);

		int CalculateNumbersAsDigits(IEnumerable<Token> tokens);

		int CalculateNumbersAsWords(IEnumerable<Token> tokens);

		double CalculatePercentageEmotionWords(IEnumerable<Token> tokens);

		int CalculateQuestionWords(IEnumerable<Token> tokens);
	}
}