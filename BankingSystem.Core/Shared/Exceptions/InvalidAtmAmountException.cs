namespace BankingSystem.Core.Shared.Exceptions
{
    public class InvalidAtmAmountException : DomainException
    {
        private static readonly int _statusCode = 400;
        public InvalidAtmAmountException(string message, params object?[]? parameters) : base(message, _statusCode, parameters) { }
    }
}
