

namespace BankingSystem.Core.Shared.Exceptions
{
    public class BankAccountsAlreadyExistException : DomainException
    {
        public BankAccountsAlreadyExistException(string message, params object?[]? parameters) : base(message, parameters)
        {

        }
    }
}
