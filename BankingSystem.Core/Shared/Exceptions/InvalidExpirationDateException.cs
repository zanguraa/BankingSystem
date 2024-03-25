
namespace BankingSystem.Core.Shared.Exceptions
{
    public class InvalidExpirationDateException : DomainException
    {
        public InvalidExpirationDateException(string message)
            : base(message)
        {
        }

    }

}
