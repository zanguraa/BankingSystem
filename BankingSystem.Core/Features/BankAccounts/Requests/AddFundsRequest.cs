using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.BankAccounts.Requests
{
    public class AddFundsRequest
    {
        public string Iban { get; set; }
        public decimal Amount { get; set; }
        public CurrencyType Currency { get; set; }
    }
}
