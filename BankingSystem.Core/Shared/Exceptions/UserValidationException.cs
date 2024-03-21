using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Shared.Exceptions
{
    public class UserValidationException : DomainException
    {
        public UserValidationException(string message) : base(message)
        {

        }
    }
}
