using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace BankingSystem.Core.Shared
{
    public interface ISeqLogger
    {
        void LogError(string message, params object?[]? parameters);
        void LogFatal(string message, params object?[]? parameters);
        void LogInfo(string message, params object?[]? parameters);
    }

    public class SeqLogger : ISeqLogger
    {
        private readonly Logger _logger;

        public SeqLogger()
        {
            _logger = SetupLogger(LogEventLevel.Information);
        }

        public void LogError(string message, params object?[]? parameters) => _logger.Error(message, parameters);
        public void LogFatal(string message, params object?[]? parameters) => _logger.Fatal(message, parameters);
        public void LogInfo(string message, params object?[]? parameters) => _logger.Information(message, parameters);


        private Logger SetupLogger(LogEventLevel logEventLevel)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Is(logEventLevel)
                .WriteTo.Seq("http://localhost:5341/")
                .CreateLogger();
        }
    }

}
