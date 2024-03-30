
namespace BankingSystem.Core.Shared.Exceptions
{
    public class InvalidAccountException : DomainException
    {
        public InvalidAccountException(string message, params object?[]? parameters) : base(message, parameters)
        {
            
        }
    }
}
