using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Atm.CardAuthorizations.Requests
{
	public class CardAuthorizationResponse
	{
		public bool IsAuthorized { get; set; }
		public string Message { get; set; }
	}
}
