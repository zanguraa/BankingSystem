namespace BankingSystem.Core.Shared.Exceptions
{
    public class InvalidAddFundsValidationException : DomainException
    {
        private static readonly int _statusCode = 400;
        public InvalidAddFundsValidationException(string message, params object?[]? parameters) : base(message, _statusCode, parameters) { }
    }
}
