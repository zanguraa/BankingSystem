namespace BankingSystem.Core.Shared.Exceptions
{
    public class InvalidBalanceException : DomainException
    {
        private static readonly int _statusCode = 400; 
        public InvalidBalanceException(string message, params object?[]? parameters) : base(message, _statusCode, parameters) { }
    }
}
