
namespace BankingSystem.Core.Shared.Exceptions
{
    public class InvalidCardException : DomainException
    {
        public InvalidCardException(string message, params object?[]? parameters) : base(message, parameters)
        {

        }
    }
}
