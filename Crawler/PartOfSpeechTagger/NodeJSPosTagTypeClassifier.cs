namespace Crawler.PartOfSpeechTagger
{
	public class NodeJSPosTagTypeClassifier : IPosTagTypeClassifier
	{
		public ePosTagType Classify(string extendedTag)
		{
			switch (extendedTag)
			{
				case "CC":
				case "IN":
					return ePosTagType.Conjunction;
				case "DT":
				case "WDT":
					return ePosTagType.Determiner;
				case "JJ":
				case "JJR":
				case "JJS":
					return ePosTagType.Adjective;
				case "NN":
				case "NNS":
				case "NNP":
				case "NNPS":
					return ePosTagType.Noun;
				case "PRP":
				case "PRP$":
				case "WP":
				case "WP$":
					return ePosTagType.Pronoun;
				case "RB":
				case "RBR":
				case "RBS":
				case "WRB":
					return ePosTagType.Adverb;
				case "VB":
				case "VBD":
				case "VBG":
				case "VBN":
				case "VBP":
				case "VBZ":
					return ePosTagType.Verb;
				default:
					return ePosTagType.Alien;
			}
		}
	}
}
