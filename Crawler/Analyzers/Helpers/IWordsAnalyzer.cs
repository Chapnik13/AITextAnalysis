using System.Collections.Generic;
using Crawler.DeJargonizer;
using Crawler.LexicalAnalyzer;

namespace Crawler.Analyzers.Helpers
{
    public interface IWordsAnalyzer
    {
        int CountWords(List<Token> tokens);
        float CalculateAverageLength(List<Token> tokens);
        double CalculateWordsLengthStandardDeviation(List<Token> tokens);
        DeJargonizerResult CalculateDeJargonizer(List<Token> tokens);
        int CalculateNumbersAsDigits(List<Token> tokens);
        int CalculateNumbersAsWords(List<Token> tokens);
        double CalculatePercentageEmotionWords(List<Token> tokens);
        int CalculateQuestionWords(List<Token> tokens);
    }
}