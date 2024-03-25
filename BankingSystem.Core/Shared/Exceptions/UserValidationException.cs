namespace BankingSystem.Core.Shared.Exceptions
{
    public class UserValidationException : DomainException
    {
        public UserValidationException(string message, params object?[]? parameters) : base(message, parameters)
        {

        }
    }
}
