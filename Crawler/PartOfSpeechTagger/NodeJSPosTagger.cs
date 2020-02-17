using Crawler.ExtensionMethods;
using Crawler.LexicalAnalyzer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace Crawler.PartOfSpeechTagger
{
	public class NodeJSPosTagger : IPosTagger
	{
		private const char SEPERATOR = ' ';

		private readonly IPosTagTypeClassifier posTagTypeClassifier;

		public NodeJSPosTagger(IPosTagTypeClassifier posTagTypeClassifier)
		{
			this.posTagTypeClassifier = posTagTypeClassifier;
		}

		public List<PosTagToken> Tag(List<Token> tokens)
		{
			var text = string.Join(SEPERATOR, tokens.GetValuesByTokenTypes(eTokenType.StringValue));
			var posTaggerPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../PosTagger/app.js"));

			var outputStream = RunPosTagProcess(posTaggerPath, text);

			var wordsPosTokensJson = outputStream.ReadToEnd();

			var wordsPosTokens = JsonSerializer.Deserialize<List<PosTagToken>>(wordsPosTokensJson);

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
