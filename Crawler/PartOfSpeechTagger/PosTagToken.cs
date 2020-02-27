using System.Text.Json.Serialization;

namespace Crawler.PartOfSpeechTagger
{
	public class PosTagToken
	{
		[JsonPropertyName("value")]
		public string Value { get; set; }

		public ePosTagType Type { get; set; }

		[JsonPropertyName("pos")]
		public ePosTagExtendedType ExtendedType { get; set; }
	}
}
