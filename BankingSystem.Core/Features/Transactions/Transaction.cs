using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Transactions
{
	public class Transaction
	{
		[Key]
		public int TransactionId { get; set; }
		public int FromAccountId { get; set; }
		public int ToAccountId { get; set; }
		public string FromAccountCurrency { get; set; }
		public string ToAccountCurrency { get; set; }
		public decimal Amount { get; set; }
		public decimal Fee { get; set; }
		public DateTime TransactionDate { get; set; }

		[ForeignKey("FromAccountId")]
		public virtual Account FromAccount { get; set; }

		[ForeignKey("ToAccountId")]
		public virtual Account ToAccount { get; set; }
	}

}
