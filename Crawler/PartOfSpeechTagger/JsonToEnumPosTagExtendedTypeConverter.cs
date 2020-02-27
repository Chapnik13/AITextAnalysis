using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Crawler.PartOfSpeechTagger
{
	public class JsonToEnumPosTagExtendedTypeConverter : JsonConverter<ePosTagExtendedType>
	{
		private readonly IPosTagExtendedTypeClassifier extendedTypeClassifier;

		public JsonToEnumPosTagExtendedTypeConverter(IPosTagExtendedTypeClassifier extendedTypeClassifier)
		{
			this.extendedTypeClassifier = extendedTypeClassifier;
		}

		public override bool CanConvert(Type typeToConvert)
		{
			return typeToConvert == typeof(ePosTagExtendedType);
		}

		public override ePosTagExtendedType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.String)
			{
				var value = reader.GetString();
				
				return extendedTypeClassifier.Classify(value);
			}

			return ePosTagExtendedType.Unknown;
		}

		public override void Write(Utf8JsonWriter writer, ePosTagExtendedType value, JsonSerializerOptions options)
		{
			throw new NotImplementedException();
		}
	}
}