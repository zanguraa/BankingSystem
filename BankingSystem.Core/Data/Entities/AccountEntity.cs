using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Data
{
	public class AccountEntity
	{
		public int Id { get; set; }
		public string? Iban { get; set; }
		public decimal Amount { get; set; }
		public string? Currency { get; set; }
		public int UserId { get; set; }
		public virtual UserEntity? User { get; set; }
	}
}
