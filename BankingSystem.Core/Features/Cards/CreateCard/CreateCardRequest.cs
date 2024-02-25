using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Cards.CreateCard
{
	public class CreateCardRequest
	{
		public int UserId { get; set; }
		public string? CardNumber { get; set; }  
		public string? AccountId { get; set; }
		public string? CardHolderName { get; set; }
		public DateTime ExpirationDate { get; set; }
		public string? CVV { get; set; }
		public string? PIN { get; set; }
	}
}
