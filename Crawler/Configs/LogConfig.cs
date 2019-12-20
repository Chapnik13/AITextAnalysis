using Serilog.Events;

namespace Crawler.Configs
{
    public class LogConfig
    {
        public bool WriteToConsole { get; set; }
        public LogEventLevel MinimumConsoleLevel { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public LogEventLevel MinimumFileLevel { get; set; }
    }
}
