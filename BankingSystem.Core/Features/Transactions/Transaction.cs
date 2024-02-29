using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Transactions
{
	public class Transaction
	{
		public int Id { get; set; }
		public int FromAccountId { get; set; }
		public int ToAccountId { get; set; }
		public string FromAccountCurrency { get; set; }
		public string ToAccountCurrency { get; set; }
		public decimal FromAmount { get; set; }
		public decimal ToAmount { get; set; }
		public decimal Fee { get; set; }
		public DateTime TransactionDate { get; set; }
		public string TransactionType { get; set; }
		public bool AddFunds { get; set; }
	}

}
