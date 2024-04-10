

namespace BankingSystem.Core.Shared.Exceptions
{
    public class BankAccountsAlreadyExistException : DomainException
    {
        private static readonly int _statusCode = 409;

        public BankAccountsAlreadyExistException(string message, params object?[]? parameters) : base(message, _statusCode, parameters)
        {

        }
    }
}
