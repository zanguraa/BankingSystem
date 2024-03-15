using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Atm.CardAuthorizations.Requests
{
	public class CardAuthorizationRequest
	{
		public string CardNumber { get; set; }
		public string Pin { get; set; }

	}
}
