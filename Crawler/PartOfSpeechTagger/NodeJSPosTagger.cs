using Crawler.LexicalAnalyzer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Crawler.PartOfSpeechTagger
{
	public class NodeJSPosTagger : IPosTagger
	{
		private const char SEPERATOR = ' ';

		private readonly IPosTagTypeClassifier posTagTypeClassifier;
		private readonly JsonConverter<ePosTagExtendedType> jsonToEnumPosTagExtendedTypeConverter;

		public NodeJSPosTagger(IPosTagTypeClassifier posTagTypeClassifier, JsonConverter<ePosTagExtendedType> jsonToEnumPosTagExtendedTypeConverter)
		{
			this.posTagTypeClassifier = posTagTypeClassifier;
			this.jsonToEnumPosTagExtendedTypeConverter = jsonToEnumPosTagExtendedTypeConverter;
		}

		public List<PosTagToken> Tag(List<Token> tokens)
		{
			var text = string.Join(SEPERATOR, tokens.Select(token => token.Value)).Replace('"', ' ');
			var posTaggerPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../PosTagger/app.js"));

			var outputStream = RunPosTagProcess(posTaggerPath, text);

			var wordsPosTokensJson = outputStream.ReadToEnd();

			var jsonSerializerOptions = new JsonSerializerOptions
			{
				Converters =
				{
					jsonToEnumPosTagExtendedTypeConverter
				}
			};

			var wordsPosTokens = JsonSerializer.Deserialize<List<PosTagToken>>(wordsPosTokensJson, jsonSerializerOptions);

			wordsPosTokens.ForEach(posToken =>
			{
				posToken.Type = posTagTypeClassifier.Classify(posToken.ExtendedType);
			});

			return wordsPosTokens;
		}

		private StreamReader RunPosTagProcess(string posTaggerPath, string text)
		{
			using var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "node",
					Arguments = $"{posTaggerPath} \"{text}\"",
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					RedirectStandardInput = true,
					CreateNoWindow = false
				}
			};

			process.Start();

			return process.StandardOutput;
		}
	}
}
