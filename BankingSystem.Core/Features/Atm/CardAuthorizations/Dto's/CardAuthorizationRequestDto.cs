using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Atm.CardAuthorizations.Dto_s
{
	public class CardAuthorizationRequestDto
	{
		public string CardNumber { get; set; }
		public string Pin { get; set; }

	}
}
