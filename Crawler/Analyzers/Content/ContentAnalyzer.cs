using Crawler.Analyzers.UtilAnalyzers;
using Crawler.LexicalAnalyzer;
using Crawler.Models;
using System.Collections.Generic;
using System.Linq;
using Crawler.ExtensionMethods;
using Crawler.PartOfSpeechTagger;

namespace Crawler.Analyzers.Content
{
	public class ContentAnalyzer : IAnalyzer<ContentAnalysisResult>
	{
		private readonly IWordsAnalyzer wordsAnalyzer;
		private readonly IPunctuationAnalyzer punctuationAnalyzer;
		private readonly IParagraphsAnalyzer paragraphAnalyzer;
		private readonly ISentencesAnalyzer sentencesAnalyzer;

		public ContentAnalyzer(IWordsAnalyzer wordsAnalyzer, IPunctuationAnalyzer punctuationAnalyzer,
			IParagraphsAnalyzer paragraphAnalyzer, ISentencesAnalyzer sentencesAnalyzer)
		{
			this.wordsAnalyzer = wordsAnalyzer;
			this.punctuationAnalyzer = punctuationAnalyzer;
			this.paragraphAnalyzer = paragraphAnalyzer;
			this.sentencesAnalyzer = sentencesAnalyzer;
		}

		public ContentAnalysisResult Analyze(Article<List<Token>> article)
		{
			var contentAsParagraphs = article.Content;
			var contentAsText = contentAsParagraphs.SelectMany(t => t).ToList();
			var deJargonizerResult = wordsAnalyzer.CalculateDeJargonizer(contentAsText);

			var posTags = wordsAnalyzer.CalculatePartOfSpeechTags(contentAsText);

			return new ContentAnalysisResult
			{
				AmountOfWords = wordsAnalyzer.CountWords(contentAsText),
				AmountOfNumbersAsWords = wordsAnalyzer.CalculateNumbersAsWords(contentAsText),
				AmountOfNumbersAsDigits = wordsAnalyzer.CalculateNumbersAsDigits(contentAsText),
				AmountOfQuestionWords = wordsAnalyzer.CalculateQuestionWords(contentAsText),
				PercentageOfEmotionWords = wordsAnalyzer.CalculateEmotionWordsPercentage(contentAsText) * 100,
				WordLengthStandardDeviation = wordsAnalyzer.CalculateWordsLengthStandardDeviation(contentAsText),
				DeJargonizerScore = deJargonizerResult.Score,
				AmountOfRareWords = deJargonizerResult.RareWords.Count,
				AverageLengthOfParagraph = paragraphAnalyzer.CalculateAverageLength(contentAsParagraphs),
				AverageAmountOfSentencesInParagraph = paragraphAnalyzer.CalculateAverageAmountOfSentences(contentAsParagraphs),
				AverageAmountOfCommasAndPeriodsInParagraph = paragraphAnalyzer.CalculateAverageAmountOfCommasAndPeriods(contentAsParagraphs),
				AverageAmountOfWordsBetweenPunctuation = punctuationAnalyzer.CalculateAverageWordsCountBetweenPunctuation(contentAsText),
				MaxAmountOfWordsBetweenPunctuation = punctuationAnalyzer.CalculateMaxWordsCountBetweenPunctuation(contentAsText),
				AmountOfWordsBetweenPunctuationStandardDeviation = punctuationAnalyzer.CalculateWordsCountBetweenPunctuationStandardDeviation(contentAsText),
				SecondDecileBetweenPunctuation = punctuationAnalyzer.CalculateWordsCountDecile(2, contentAsText),
				NinthDecileBetweenPunctuation = punctuationAnalyzer.CalculateWordsCountDecile(9, contentAsText),
				PassiveVoiceSentencesPercentage = sentencesAnalyzer.CalculatePassiveVoiceSentencesPercentage(posTags),
				AmountOfQuestionMarks = punctuationAnalyzer.CountCharacter('?', contentAsText),
				AmountOfExclamationMarks = punctuationAnalyzer.CountCharacter('!', contentAsText),
				AmountOfDashes = punctuationAnalyzer.CountCharacter('-', contentAsText),
				AmountOfColons = punctuationAnalyzer.CountCharacter(':', contentAsText),
				AmountOfQuotationMarks = punctuationAnalyzer.CountCharacter('"', contentAsText),
				AmountOfAdjectives = posTags.CountPosTagTokensByPosTagTokenTypes(ePosTagType.Adjective),
				AmountOfAdverbs = posTags.CountPosTagTokensByPosTagTokenTypes(ePosTagType.Adverb),
				AmountOfConjunctions = posTags.CountPosTagTokensByPosTagTokenTypes(ePosTagType.Conjunction),
				AmountOfDeterminers = posTags.CountPosTagTokensByPosTagTokenTypes(ePosTagType.Adjective),
				AmountOfNouns = posTags.CountPosTagTokensByPosTagTokenTypes(ePosTagType.Noun),
				AmountOfPronouns = posTags.CountPosTagTokensByPosTagTokenTypes(ePosTagType.Pronoun),
				AmountOfVerbs = posTags.CountPosTagTokensByPosTagTokenTypes(ePosTagType.Verb),
				AmountOfUnknowns = posTags.CountPosTagTokensByPosTagTokenTypes(ePosTagType.Unknown)
			};

		}
	}
}