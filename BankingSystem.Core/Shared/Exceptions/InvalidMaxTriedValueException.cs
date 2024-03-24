
namespace BankingSystem.Core.Shared.Exceptions
{
    public class InvalidMaxTriedValueException : DomainException
    {
        public InvalidMaxTriedValueException(string message)
            : base(message)
        {
        }
    }

}
