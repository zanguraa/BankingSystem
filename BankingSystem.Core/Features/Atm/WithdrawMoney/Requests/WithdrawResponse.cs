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
	}
}
