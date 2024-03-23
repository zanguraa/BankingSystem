namespace BankingSystem.Core.Shared.Exceptions
{
    public class InvalidAddFundsValidationException : DomainException
    {
        public InvalidAddFundsValidationException(string message) : base(message)
        {

        }
    }
}
