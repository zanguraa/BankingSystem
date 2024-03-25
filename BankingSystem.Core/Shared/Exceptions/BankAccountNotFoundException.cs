
namespace BankingSystem.Core.Shared.Exceptions
{
    public class BankAccountNotFoundException : DomainException
    {
        public BankAccountNotFoundException(string message) : base(message)
        {

        }
    }
}
