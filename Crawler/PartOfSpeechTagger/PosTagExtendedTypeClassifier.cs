namespace Crawler.PartOfSpeechTagger
{
	public class PosTagExtendedTypeClassifier : IPosTagExtendedTypeClassifier
	{
		public ePosTagExtendedType Classify(string extendedTag)
		{
			return extendedTag switch
			{
				"CC" => ePosTagExtendedType.CoordinatingConjunction,
				"CD" => ePosTagExtendedType.CardinalNumber,
				"DT" => ePosTagExtendedType.Determiner,
				"EX" => ePosTagExtendedType.Existential,
				"FW" => ePosTagExtendedType.ForeignWord,
				"IN" => ePosTagExtendedType.PrepositionOrSubordinatingConjunction,
				"JJ" => ePosTagExtendedType.Adjective,
				"JJR" => ePosTagExtendedType.ComparativeAdjective,
				"JJS" => ePosTagExtendedType.SuperlativeAdjective,
				"LS" => ePosTagExtendedType.ListItemMarker,
				"MD" => ePosTagExtendedType.Modal,
				"NN" => ePosTagExtendedType.SingularOrMassNoun,
				"NNS" => ePosTagExtendedType.PluralNoun,
				"NNP" => ePosTagExtendedType.SingularProperNoun,
				"NNPS" => ePosTagExtendedType.PluralProperNoun,
				"PDT" => ePosTagExtendedType.Predeterminer,
				"POS" => ePosTagExtendedType.PossessiveEnding,
				"PRP" => ePosTagExtendedType.PersonalPronoun,
				"PRP$" => ePosTagExtendedType.PossessivePronoun,
				"RB" => ePosTagExtendedType.Adverb,
				"RBR" => ePosTagExtendedType.ComparativeAdverb,
				"RBS" => ePosTagExtendedType.SuperlativeAdverb,
				"RP" => ePosTagExtendedType.Particle,
				"SYM" => ePosTagExtendedType.Symbol,
				"TO" => ePosTagExtendedType.To,
				"UH" => ePosTagExtendedType.Interjection,
				"VB" => ePosTagExtendedType.BaseFormVerb,
				"VBD" => ePosTagExtendedType.PastTenseVerb,
				"VBG" => ePosTagExtendedType.GerundOrPresentParticipleVerb,
				"VBN" => ePosTagExtendedType.PastParticipleVerb,
				"VBP" => ePosTagExtendedType.NonThirdPersonSingularPresentVerb,
				"VBZ" => ePosTagExtendedType.ThirdPersonSingularPresentVerb,
				"WDT" => ePosTagExtendedType.WhDeterminer,
				"WP" => ePosTagExtendedType.WhPronoun,
				"WP$" => ePosTagExtendedType.PossessiveWhPronoun,
				"WRB" => ePosTagExtendedType.WhAdverb,
				"." => ePosTagExtendedType.EndOfSentence,
				_ => ePosTagExtendedType.Unknown
			};
		}
	}
}
