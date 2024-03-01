using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Transactions.CreateTransaction
{
	public class CreateTransactionRequest
	{
		public int FromAccountId { get; set; }
		public int ToAccountId { get; set; }
		public decimal Amount { get; set; }
		public string Currency { get; set; }
		public string ToCurrency { get; set; }
		public decimal Fee { get; set; }
		public DateTime TransactionDate { get; set; }
		public CreateTransactionRequest()
		{
			TransactionDate = DateTime.UtcNow;
		}
	}
}
