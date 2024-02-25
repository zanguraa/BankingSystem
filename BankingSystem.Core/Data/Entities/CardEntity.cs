using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Data.Entities
{
    public class CardEntity
    {
		public int Id { get; set; }
		public string CardNumber { get; set; }
		public string FullName { get; set; }
		public DateTime ExpirationDate { get; set; }
		public int Cvv { get; set; }
		public int Pin { get; set; }
		public int MaxTried { get; set; }
		public bool IsLocked { get; set; }
		public DateTime CreatedAt { get; set; }

		public int UserId { get; set; }
		public int AccountId { get; set; }

		

	}
}
