namespace Crawler.PartOfSpeechTagger
{
	public interface IPosTagTypeClassifier
	{
		ePosTagType Classify(ePosTagExtendedType extendedTag);
	}
}