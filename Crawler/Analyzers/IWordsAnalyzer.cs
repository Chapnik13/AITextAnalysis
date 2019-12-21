using System.Collections.Generic;
using Crawler.DeJargonizer;
using Crawler.LexicalAnalyzer;

namespace Crawler.Analyzers
{
	public interface IWordsAnalyzer
	{
		float CalculateAverageLength(IEnumerable<Token> tokens);

		double CalculateStandardDeviation(IEnumerable<Token> tokens);

		DeJargonizerResult CalculateDeJargonizer(IEnumerable<Token> tokens);
	}
}