using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Atm.WithdrawMoney
{
	public class DailyWithdrawal
	{
		public int DailyWithdrawalId { get; set; }
		public int BankAccountId { get; set; }
		public DateTime WithdrawalDate { get; set; }
		public decimal TotalAmount { get; set; }
		public string? Currency { get; set; }
	}
}
