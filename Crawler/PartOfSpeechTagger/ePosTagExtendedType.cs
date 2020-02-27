using System.Text.Json.Serialization;

namespace Crawler.PartOfSpeechTagger
{
	public enum ePosTagExtendedType
	{
		CoordinatingConjunction,
		CardinalNumber,
		Determiner,
		Existential,
		ForeignWord,
		PrepositionOrSubordinatingConjunction,
		Adjective,
		ComparativeAdjective,
		SuperlativeAdjective,
		ListItemMarker,
		Modal,
		SingularOrMassNoun,
		PluralNoun,
		SingularProperNoun, 
		PluralProperNoun,
		Predeterminer,
		PossessiveEnding,
		PersonalPronoun,
		PossessivePronoun,
		Adverb,
		ComparativeAdverb,
		SuperlativeAdverb, 
		Particle,
		Symbol,
		To,
		Interjection,
		BaseFormVerb,
		PastTenseVerb,
		GerundOrPresentParticipleVerb,
		PastParticipleVerb,
		NonThirdPersonSingularPresentVerb, 
		ThirdPersonSingularPresentVerb, 
		WhDeterminer,
		WhPronoun,
		PossessiveWhPronoun,
		WhAdverb,
		EndOfSentence,
		Unknown
	}
}
