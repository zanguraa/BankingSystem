
namespace BankingSystem.Core.Shared.Exceptions
{
    public class InvalidCardException : DomainException
    {
        private static readonly int _statusCode = 400; 
        public InvalidCardException(string message, params object?[]? parameters) : base(message, _statusCode, parameters) { }
    }
}
