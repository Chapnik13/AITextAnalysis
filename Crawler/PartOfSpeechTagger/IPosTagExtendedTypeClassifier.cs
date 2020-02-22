namespace Crawler.PartOfSpeechTagger
{
	public interface IPosTagExtendedTypeClassifier
	{
		ePosTagExtendedType Classify(string extendedTag);
	}
}