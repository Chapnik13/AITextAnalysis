namespace Crawler.PartOfSpeechTagger
{
	public interface IPosTagTypeClassifier
	{
		ePosTagType Classify(string extendedTag);
	}
}