using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Models.Requests
{
	public class CreateAccountRequest
	{
		public string Iban { get; set; }
		public decimal InitialAmount { get; set; }
		public string Currency { get; set; }

	}
}
