namespace BankingSystem.Core.Shared.Exceptions
{
    public class InvalidBalanceException : DomainException
    {
        public InvalidBalanceException(string message, params object?[]? parameters) : base(message, parameters)
        {

        }
    }
}
