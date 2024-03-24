

namespace BankingSystem.Core.Shared.Exceptions
{
    public class BankAccountsAlreadyExistException : DomainException
    {
        public BankAccountsAlreadyExistException(string message) : base(message)
        {

        }
    }
}
