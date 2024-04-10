namespace BankingSystem.Core.Shared.Exceptions
{
    public class UserValidationException : DomainException
    {
        private static readonly int _statusCode = 404; 
        public UserValidationException(string message, params object?[]? parameters) : base(message, _statusCode, parameters) { }
    }
}
