namespace BankingSystem.Core.Shared.Exceptions
{
    public class InvalidAtmAmountException : DomainException
    {
        public InvalidAtmAmountException(string message, params object?[]? parameters) : base(message, parameters)
        {

        }
    }
}
