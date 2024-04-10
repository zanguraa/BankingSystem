
namespace BankingSystem.Core.Shared.Exceptions
{
    public class BankAccountNotFoundException : DomainException
    {
        private static readonly int _statusCode = 404;
        public BankAccountNotFoundException(string message, params object?[]? parameters) : base(message, _statusCode, parameters)
        {

        }

    }
}
