using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Cards.CreateCard
{
	public class CreateCardRequest
	{
		public string FullName { get; set; }
		public DateTime ExpirationDate { get; set; }
		public int Cvv { get; set; }
		public int Pin { get; set; }
		public int MaxTried { get; set; }
		public int UserId { get; set; }  
		public int AccountId { get; set; }
	}
}
