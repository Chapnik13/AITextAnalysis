using System;
using AngleSharp;
using Crawler.LexicalAnalyzer;
using Crawler.MockupWrappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Threading.Tasks;
using Crawler.Configs;
using Serilog.Events;

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
            services.AddHostedService<CrawlerService>();
            services.AddTransient<IBrowsingContext>(_ => BrowsingContext.New(Configuration.Default.WithDefaultLoader()));
            services.AddTransient<IBrowsingContextWrapper, BrowsingContextWrapper>();
            services.AddTransient<IScienceDailyScraper, ScienceDailyScraper>();
            services.AddTransient<ILexer, Lexer>();
        }

        private static void ConfigureLogging(HostBuilderContext context, LoggerConfiguration logging)
        {
            var logConfig = new LogConfig();
            context.Configuration.GetSection("Logging").Bind(logConfig);

            logging
                .MinimumLevel.Verbose()
                .WriteTo.Conditional(_ => logConfig.WriteToConsole, configuration => configuration.Console(logConfig.MinimumConsoleLevel))
                .WriteTo.File(logConfig.FilePath, logConfig.MinimumFileLevel);
        }
    }
}
