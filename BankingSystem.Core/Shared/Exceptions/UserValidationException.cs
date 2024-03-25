namespace BankingSystem.Core.Shared.Exceptions
{
    public class UserValidationException : DomainException
    {
        public UserValidationException(string message) : base(message)
        {

        }
    }
}
