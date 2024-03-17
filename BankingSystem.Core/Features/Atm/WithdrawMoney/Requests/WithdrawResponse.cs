using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Atm.WithdrawMoney.Requests
{
	public class WithdrawResponse
	{
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public decimal RemainingBalance { get; set; }
        public decimal Commission { get; set; } 
        public decimal RequestedAmount { get; set; }
        public string RequestedCurrency { get; set; }
        public decimal DeductedAmount { get; set; }
        public string AccountCurrency { get; set; }
        public DateTime WithdrawalDate { get; set; }

    }
}
