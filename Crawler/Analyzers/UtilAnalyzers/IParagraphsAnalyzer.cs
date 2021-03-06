﻿using Crawler.LexicalAnalyzer;
using System.Collections.Generic;

namespace Crawler.Analyzers.UtilAnalyzers
{
    public interface IParagraphsAnalyzer
    {
        float CalculateAverageLength(List<List<Token>> paragraphs);
        float CalculateAverageAmountOfCommasAndPeriods(List<List<Token>> paragraphs);
        float CalculateAverageAmountOfSentences(List<List<Token>> paragraphs);
    }
}