using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Atm.ChangePin.Dto_s
{
	public class ChangePinRequestDto
	{
		public string CardNumber { get; set; }
		public string CurrentPin { get; set; }
		public string NewPin { get; set; }
	}

}
