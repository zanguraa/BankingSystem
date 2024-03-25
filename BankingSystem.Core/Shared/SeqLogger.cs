using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace BankingSystem.Core.Shared
{
    public interface ISeqLogger
    {
        void LogError(string message, params object?[]? parameters);
        void LogFatal(string message, params object?[]? parameters);

    }

    public class SeqLogger : ISeqLogger
    {
        private readonly Logger _errorLogger;
        private readonly Logger _fataLogger;

        public SeqLogger()
        {
            _errorLogger = SetupLogger(LogEventLevel.Error);
            _fataLogger = SetupLogger(LogEventLevel.Fatal);
        }

        public void LogError(string message, params object?[]? parameters) => _errorLogger.Error(message, parameters);

        public void LogFatal(string message, params object?[]? parameters) => _fataLogger.Fatal(message, parameters);

        private Logger SetupLogger(LogEventLevel logEventLevel)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Is(logEventLevel)
                .WriteTo.Seq("http://localhost:5341/")
                .CreateLogger();
        }
    }

}
