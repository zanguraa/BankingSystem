namespace BankingSystem.Core.Shared.Exceptions
{
    public class InvalidAddFundsValidationException : DomainException
    {
        public InvalidAddFundsValidationException(string message, params object?[]? parameters) : base(message, parameters)
        {

        }
    }
}
