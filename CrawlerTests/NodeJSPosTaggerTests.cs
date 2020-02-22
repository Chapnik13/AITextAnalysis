using Crawler.LexicalAnalyzer;
using Crawler.PartOfSpeechTagger;
using System.Collections.Generic;
using Xunit;

namespace CrawlerTests
{
	public class NodeJSPosTaggerTests
	{
		private readonly NodeJSPosTagger posTagger;

		public NodeJSPosTaggerTests()
		{
			posTagger = new NodeJSPosTagger(new PosTagTypeClassifier(), new JsonToEnumPosTagExtendedTypeConverter(new PosTagExtendedTypeClassifier()));
		}

		[Fact]
		public void Tag_ShouldReturnEmptyList_WhenEmptyTokenList()
		{
			var result = posTagger.Tag(new List<Token>());

			Assert.Equal(new List<PosTagToken>(), result);
		}

		[Theory]
		[InlineData("food")]
		[InlineData("Ancient")]
		[InlineData("Rapanui")]
		[InlineData("carvers")]
		[InlineData("elite")]
		[InlineData("ruling")]
		[InlineData("class")]
		[InlineData("Moai")]
		public void Tag_ShouldReturnOneTag_WhenOneToken(string word)
		{
			var result = posTagger.Tag(new List<Token> { new Token(eTokenType.StringValue, word) }).Count;

			Assert.Equal(1, result);
		}

		[Theory]
		[InlineData("capable")]
		[InlineData("large")]
		[InlineData("agricultural")]
		[InlineData("critical")]
		[InlineData("new")]
		public void Tag_ShouldReturnAdjectiveTag_WhenOneToken(string word)
		{
			var result = posTagger.Tag(new List<Token>{new Token(eTokenType.StringValue, word)})[0].Type;

			Assert.Equal(ePosTagType.Adjective, result);
		}

		[Theory]
		[InlineData("recently")]
		[InlineData("nearly")]
		[InlineData("thereby")]
		public void Tag_ShouldReturnAdverbTag_WhenOneToken(string word)
		{
			var result = posTagger.Tag(new List<Token> { new Token(eTokenType.StringValue, word) })[0].Type;

			Assert.Equal(ePosTagType.Adverb, result);
		}

		[Theory]
		[InlineData("TO")]
		[InlineData("1,000")]
		public void Tag_ShouldReturnUnknownTag_WhenOneToken(string word)
		{
			var result = posTagger.Tag(new List<Token> { new Token(eTokenType.StringValue, word) })[0].Type;

			Assert.Equal(ePosTagType.Unknown, result);
		}

		[Theory]
		[InlineData("in")]
		[InlineData("from")]
		[InlineData("at")]
		[InlineData("because")]
		[InlineData("of")]
		public void Tag_ShouldReturnConjunctionTag_WhenOneToken(string word)
		{
			var result = posTagger.Tag(new List<Token> { new Token(eTokenType.StringValue, word) })[0].Type;

			Assert.Equal(ePosTagType.Conjunction, result);
		}

		[Theory]
		[InlineData("a")]
		[InlineData("the")]
		public void Tag_ShouldReturnDeterminerTag_WhenOneToken(string word)
		{
			var result = posTagger.Tag(new List<Token> { new Token(eTokenType.StringValue, word) })[0].Type;

			Assert.Equal(ePosTagType.Determiner, result);
		}

		[Theory]
		[InlineData("they")]
		public void Tag_ShouldReturnPronounTag_WhenOneToken(string word)
		{
			var result = posTagger.Tag(new List<Token> { new Token(eTokenType.StringValue, word) })[0].Type;

			Assert.Equal(ePosTagType.Pronoun, result);
		}

		[Theory]
		[InlineData("according")]
		[InlineData("producing")]
		[InlineData("believed")]
		[InlineData("carve")]
		[InlineData("worked")]
		[InlineData("published")]
		public void Tag_ShouldReturnVerbTag_WhenOneToken(string word)
		{
			var result = posTagger.Tag(new List<Token> { new Token(eTokenType.StringValue, word) })[0].Type;

			Assert.Equal(ePosTagType.Verb, result);
		}
	}
}
