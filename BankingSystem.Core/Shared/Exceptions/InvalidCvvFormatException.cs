

namespace BankingSystem.Core.Shared.Exceptions
{
    public class InvalidCvvFormatException : DomainException
    {
        public InvalidCvvFormatException(string message)
            : base(message)
        {
        }
    }

}
