
namespace BankingSystem.Core.Shared.Exceptions
{
    public class UserNotFoundException : DomainException
    {
        private static readonly int _statusCode = 404; 
        public UserNotFoundException(string message, params object?[]? parameters) : base(message, _statusCode, parameters) { }
    }
}
