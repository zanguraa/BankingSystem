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
        private readonly Logger _errorLogger;
        private readonly Logger _fataLogger;
        private readonly Logger _infoLogger;

        public SeqLogger()
        {
            _errorLogger = SetupLogger(LogEventLevel.Error);
            _fataLogger = SetupLogger(LogEventLevel.Fatal);
            _infoLogger = SetupLogger(LogEventLevel.Information);
        }

        public void LogError(string message, params object?[]? parameters) => _errorLogger.Error(message, parameters);
        public void LogFatal(string message, params object?[]? parameters) => _fataLogger.Fatal(message, parameters);
        public void LogInfo(string message, params object?[]? parameters) => _infoLogger.Information(message, parameters);


        private Logger SetupLogger(LogEventLevel logEventLevel)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Is(logEventLevel)
                .WriteTo.Seq("http://localhost:5341/")
                .CreateLogger();
        }
    }

}
