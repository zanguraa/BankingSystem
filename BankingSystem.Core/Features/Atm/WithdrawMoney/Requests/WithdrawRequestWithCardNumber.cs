using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Atm.WithdrawMoney.Requests
{
    public class WithdrawRequestWithCardNumber : WithdrawAmountCurrency
    {
        public string CardNumber { get; set; }
    }
}
