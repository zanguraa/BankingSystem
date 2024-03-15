using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Atm.ViewBalance.Requests
{
	public class BalanceResponse
	{
		public string? UserId { get; set; }
		public decimal InitialAmount { get; set; }
		public string? Currency { get; set; }
	}
}
