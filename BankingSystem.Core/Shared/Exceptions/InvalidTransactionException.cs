namespace BankingSystem.Core.Shared.Exceptions
{
    public class InvalidTransactionException : DomainException
    {
        private static readonly int _statusCode = 400;

        public InvalidTransactionException(string message, params object?[]? parameters) : base(message, _statusCode, parameters)
        {

        }
    }
}
