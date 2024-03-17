using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Atm.WithdrawMoney.Requests
{
    public class WithdrawAmountCurrency
    {
        public string Currency { get; set; }
        public int Amount { get; set; }
    }
}
