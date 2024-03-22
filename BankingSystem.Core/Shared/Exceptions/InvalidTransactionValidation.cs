namespace BankingSystem.Core.Shared.Exceptions
{
    public class InvalidTransactionValidation : DomainException
    {
        public InvalidTransactionValidation(string message) : base(message)
        {

        }
    }
}
