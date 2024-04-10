
namespace BankingSystem.Core.Shared.Exceptions
{
    public class InvalidAccountException : DomainException
    {
        private static readonly int _statusCode = 400;

        public InvalidAccountException(string message, params object?[]? parameters) : base(message, _statusCode, parameters)
        {

        }
    }
}
