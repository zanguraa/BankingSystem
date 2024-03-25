
namespace BankingSystem.Core.Shared.Exceptions
{
    public class UserNotFoundException : DomainException
    {
        public UserNotFoundException(string message, params object?[]? parameters) : base(message, parameters)
        {

        }
    }
}
