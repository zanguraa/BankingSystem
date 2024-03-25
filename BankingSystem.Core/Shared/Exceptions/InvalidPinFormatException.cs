
namespace BankingSystem.Core.Shared.Exceptions
{
    public class InvalidPinFormatException : DomainException
    {
        public InvalidPinFormatException(string message)
            : base(message)
        {
        }
    }

}
