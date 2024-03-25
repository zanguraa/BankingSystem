namespace BankingSystem.Core.Shared.Exceptions
{
    public class InvalidAtmAmountException : DomainException
    {
        public InvalidAtmAmountException(string message) : base(message)
        {

        }
    }
}
