using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Shared.Exceptions
{
    public class InvalidBalanceException : DomainException
    {
        public InvalidBalanceException(string message, params object?[]? parameters) : base(message, parameters)
        {

        }
    }
}
