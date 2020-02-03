using AngleSharp;
using Crawler.Analyzers;
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
                .Configure<WordsCountMatrixConfig>(context.Configuration.GetSection("WordsCountMatrix"))
                .Configure<WordsCountThresholdsConfig>(context.Configuration.GetSection("WordsCountThresholds"))
                .Configure<LexerConfig>(context.Configuration.GetSection("Lexer"))
                .Configure<ScrapersConfig>(context.Configuration.GetSection("Scrapers"));

            services.AddHostedService<CrawlerService>()
                .AddTransient<IBrowsingContext>(_ => BrowsingContext.New(Configuration.Default.WithDefaultLoader()))
                .AddTransient<IBrowsingContextWrapper, BrowsingContextWrapper>()
                .AddTransient<IScraper, Scraper>()
                .AddTransient<ILexer, Lexer>()
                .AddTransient<IWordsCountLoader, WordsCountLoader>()
                .AddTransient<IDeJargonizer, DeJargonizer.DeJargonizer>()
                .AddTransient<IWordsAnalyzer, WordsAnalyzer>()
                .AddTransient<IParagraphAnalyzer, ParagraphAnalyzer>()
                .AddTransient<IPunctuationAnalyzer, PunctuationAnalyzer>();
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
