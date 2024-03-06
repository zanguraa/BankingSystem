using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Atm.CardAuthorizations.Dto_s
{
	public class CardAuthorizationResponseDto
	{
		public bool IsAuthorized { get; set; }
		public string Message { get; set; }
	}
}
