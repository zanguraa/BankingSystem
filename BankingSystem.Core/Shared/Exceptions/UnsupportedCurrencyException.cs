namespace BankingSystem.Core.Shared.Exceptions
{
    public class UnsupportedCurrencyException : DomainException
    {
        private static readonly int _statusCode = 404; 
        public UnsupportedCurrencyException(string message, params object?[]? parameters) : base(message, _statusCode, parameters) { }
    }
}
