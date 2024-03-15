using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Atm.WithdrawMoney.Requests
{
	public class WithdrawalCheck
	{
		public int BankAccountId { get; set; }
		public DateTime WithdrawalDate { get; set; }
	}
}
