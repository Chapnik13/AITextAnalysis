namespace Crawler.PartOfSpeechTagger
{
	public class PosTagTypeClassifier : IPosTagTypeClassifier
	{
		public ePosTagType Classify(ePosTagExtendedType extendedTag)
		{
			switch (extendedTag)
			{
				case ePosTagExtendedType.CoordinatingConjunction:
				case ePosTagExtendedType.PrepositionOrSubordinatingConjunction:
					return ePosTagType.Conjunction;

				case ePosTagExtendedType.Determiner:
				case ePosTagExtendedType.WhDeterminer:
					return ePosTagType.Determiner;

				case ePosTagExtendedType.Adjective:
				case ePosTagExtendedType.ComparativeAdjective:
				case ePosTagExtendedType.SuperlativeAdjective:
					return ePosTagType.Adjective;

				case ePosTagExtendedType.SingularOrMassNoun:
				case ePosTagExtendedType.PluralNoun:
				case ePosTagExtendedType.SingularProperNoun:
				case ePosTagExtendedType.PluralProperNoun:
					return ePosTagType.Noun;

				case ePosTagExtendedType.PersonalPronoun:
				case ePosTagExtendedType.PossessivePronoun:
				case ePosTagExtendedType.WhPronoun:
				case ePosTagExtendedType.PossessiveWhPronoun:
					return ePosTagType.Pronoun;

				case ePosTagExtendedType.Adverb:
				case ePosTagExtendedType.ComparativeAdverb:
				case ePosTagExtendedType.SuperlativeAdverb:
				case ePosTagExtendedType.WhAdverb:
					return ePosTagType.Adverb;

				case ePosTagExtendedType.BaseFormVerb:
				case ePosTagExtendedType.PastTenseVerb:
				case ePosTagExtendedType.GerundOrPresentParticipleVerb:
				case ePosTagExtendedType.PastParticipleVerb:
				case ePosTagExtendedType.NonThirdPersonSingularPresentVerb:
				case ePosTagExtendedType.ThirdPersonSingularPresentVerb:
					return ePosTagType.Verb;

				default:
					return ePosTagType.Unknown;
			}
		}
	}
}
