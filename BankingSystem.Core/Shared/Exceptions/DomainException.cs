namespace BankingSystem.Core.Shared.Exceptions
{
    public class DomainException : Exception
    {
        private object?[]? parameters;

        public LogBag LogMessage { get; } = new("");
        public int StatusCode { get; set; } = 500;

        public DomainException(string message, int statusCode, params object?[]? parameters) : base(message)
        {
            StatusCode = statusCode;
            LogMessage = new LogBag(message, parameters);
        }

        public DomainException(string? message, object?[]? parameters) : base(message)
        {
            this.parameters = parameters;
        }

        public class LogBag
        {
            public string Message { get; set;  }
            public object?[]? Params { get; }
            public LogBag(string message, params object?[]? parameters)
            {
                Message = message;
                Params = parameters;
            }
        }
    }
}
