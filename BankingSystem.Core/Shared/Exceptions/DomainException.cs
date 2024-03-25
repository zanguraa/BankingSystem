namespace BankingSystem.Core.Shared.Exceptions
{
    public class DomainException : Exception
    {
        public LogBag LogMessage { get; } = new("");
        public DomainException(string message, params object?[]? parameters) : base(message)
        {
            LogMessage = new LogBag(message, parameters);
        }

        public class LogBag
        {
            public string Message { get; }
            public object?[]? Params { get; }
            public LogBag(string message, params object?[]? parameters)
            {
                Message = message;
                Params = parameters;
            }
        }
    }
}
