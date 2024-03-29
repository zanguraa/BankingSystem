namespace BankingSystem.Core.Shared.Exceptions
{
    public class UnsupportedCurrencyException : DomainException
    {
        public UnsupportedCurrencyException(string message, params object?[]? parameters) : base(message, parameters)
        {
            
        }
    }
}
