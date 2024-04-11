

using Microsoft.AspNetCore.Http;

namespace BankingSystem.Core.Shared.Exceptions
{
    public class InvalidReportsException :DomainException
    {
        private static readonly int _statusCode = 400;

        public InvalidReportsException(string message, params object?[]? parameters) : base(message, _statusCode, parameters)
        {
            
        }
    }
}
