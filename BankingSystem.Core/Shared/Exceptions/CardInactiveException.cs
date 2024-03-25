
namespace BankingSystem.Core.Shared.Exceptions
{
    public class CardInactiveException : DomainException
    {
        public CardInactiveException(string message) : base(message)
        {
            
        }
    }
}
