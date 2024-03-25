using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Shared
{
    public interface ISeqLogger
    {
        void LogError(string message, params object?[]? parameters);
    }

    public class SeqLogger : ISeqLogger
    {
        private readonly Logger _errorLogger;
        public SeqLogger()
        {
            _errorLogger = SetupLogger(LogEventLevel.Error);
        }

        public void LogError(string message, params object?[]? parameters) => _errorLogger.Error(message, parameters);
        private Logger SetupLogger(LogEventLevel logEventLevel)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Is(logEventLevel)
                .WriteTo.Seq("http://localhost:5341/")
                .CreateLogger();
        }
    }

}
