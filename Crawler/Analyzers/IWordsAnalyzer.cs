﻿using System.Collections.Generic;
using Crawler.DeJargonizer;
using Crawler.LexicalAnalyzer;

namespace Crawler.Analyzers
{
	public interface IWordsAnalyzer
	{
		float CalculateAverageLength(IEnumerable<Token> tokens);

		double CalculateStandardDeviation(IEnumerable<Token> tokens);

		DeJargonizerResult CalculateDeJargonizer(IEnumerable<Token> tokens);

        public int CalculateNumbersAsDigits(IEnumerable<Token> tokens);

        public int CalculateNumbersAsWords(IEnumerable<Token> tokens);

        public double CalculateEmotionWords(IEnumerable<Token> tokens);

        public int CalculateQuestionWords(IEnumerable<Token> tokens);
    }
}