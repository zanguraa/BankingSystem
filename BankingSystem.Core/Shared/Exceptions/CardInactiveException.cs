using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Shared.Exceptions
{
    public class CardInactiveException : DomainException
    {
        public CardInactiveException(string message) : base(message)
        {
            
        }
    }
}
