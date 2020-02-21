using AngleSharp;
using Crawler.Analyzers;
using Crawler.Analyzers.Content;
using Crawler.Analyzers.Subtitle;
using Crawler.Analyzers.Title;
using Crawler.Analyzers.UtilAnalyzers;
using Crawler.Configs;
using Crawler.DeJargonizer;
using Crawler.LexicalAnalyzer;
using Crawler.MockupWrappers;
using Crawler.SiteScraper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Threading.Tasks;
using Crawler.PartOfSpeechTagger;

namespace Crawler
{
    public static class Program
    {
        public static async Task Main()
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration(ConfigureConfiguration)
                .ConfigureServices(ConfigureServices)
                .UseSerilog(ConfigureLogging);

            await builder.RunConsoleAsync().ConfigureAwait(false);
        }

        private static void ConfigureConfiguration(HostBuilderContext context, IConfigurationBuilder config)
        {
            config
                .AddJsonFile("appsettings.json", false, false)
                .Build();
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddOptions()
                .Configure<DataFilesConfig>(context.Configuration.GetSection("DataFiles"))
                .Configure<WordsCountThresholdsConfig>(context.Configuration.GetSection("WordsCountThresholds"))
                .Configure<LexerConfig>(context.Configuration.GetSection("Lexer"))
                .Configure<ScrapersConfig>(context.Configuration.GetSection("Scrapers"));

            services.AddHostedService<CrawlerService>()
                .AddTransient<IBrowsingContext>(_ => BrowsingContext.New(Configuration.Default.WithDefaultLoader()))
                .AddTransient<IBrowsingContextWrapper, BrowsingContextWrapper>()
                .AddTransient<IScraper, Scraper>()
                .AddTransient<ILexer, Lexer>()
                .AddTransient<IWordsCountLoader, WordsCountLoader>()
                .AddTransient<IDeJargonizer, DeJargonizeAnalyzer>()
                .AddTransient<IPosTagger, NodeJSPosTagger>()
                .AddTransient<IPosTagTypeClassifier, PosTagTypeClassifier>()
                .AddTransient<IWordsAnalyzer, WordsAnalyzer>()
                .AddTransient<IParagraphsAnalyzer, ParagraphsAnalyzer>()
                .AddTransient<IPunctuationAnalyzer, PunctuationAnalyzer>()
                .AddTransient<ISentencesAnalyzer, SentencesAnalyzer>()
                .AddTransient<IAnalyzer<ContentAnalysisResult>, ContentAnalyzer>()
                .AddTransient<IAnalyzer<TitleAnalysisResult>, TitleAnalyzer>()
                .AddTransient<IAnalyzer<SubtitleAnalysisResult>, SubtitleAnalyzer>();

        }

        private static void ConfigureLogging(HostBuilderContext context, LoggerConfiguration logging)
        {
            var logConfig = new LogConfig();
            context.Configuration.GetSection("Logging").Bind(logConfig);

            logging
                .MinimumLevel.Verbose()
                .WriteTo.Conditional(_ => logConfig.WriteToConsole,
                    configuration => configuration.Console(logConfig.MinimumConsoleLevel))
                .WriteTo.File(logConfig.FilePath, logConfig.MinimumFileLevel);
        }
    }
}
