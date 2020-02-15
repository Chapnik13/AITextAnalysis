﻿using Crawler.LexicalAnalyzer;
using System.Collections.Generic;

namespace Crawler.Analyzers.Helpers
{
    public interface IParagraphAnalyzer
    {
        float CalculateAverageLength(List<List<Token>> paragraphs);
        float CalculateAverageAmountOfCommasAndPeriods(List<List<Token>> paragraphs);
        float CalculateAverageAmountOfSentences(List<List<Token>> paragraphs);
    }
}