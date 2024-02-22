using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Models.Requests
{
	public class IssueCardRequest
	{
		public int AccountId { get; set; }
		public string CardHolderName { get; set; }
		public DateTime CardExpirationDate { get; set; }

	}
}
