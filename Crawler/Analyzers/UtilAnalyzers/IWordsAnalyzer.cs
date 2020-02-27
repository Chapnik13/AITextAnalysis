using System.Collections.Generic;
using Crawler.DeJargonizer;
using Crawler.LexicalAnalyzer;
using Crawler.PartOfSpeechTagger;

namespace Crawler.Analyzers.UtilAnalyzers
{
    public interface IWordsAnalyzer
    {
        int CountWords(List<Token> tokens);

        float CalculateAverageLength(List<Token> tokens);

        double CalculateWordsLengthStandardDeviation(List<Token> tokens);

        DeJargonizerResult CalculateDeJargonizer(List<Token> tokens);

        int CalculateNumbersAsDigits(List<Token> tokens);

        int CalculateNumbersAsWords(List<Token> tokens);

        double CalculateEmotionWordsPercentage(List<Token> tokens);

        int CalculateQuestionWords(List<Token> tokens);

        List<PosTagToken> CalculatePartOfSpeechTags(List<Token> tokens);
    }
}