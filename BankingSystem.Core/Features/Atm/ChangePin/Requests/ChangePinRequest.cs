using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Atm.ChangePin.Requests
{
	public class ChangePinRequest
	{
		public string CardNumber { get; set; }
		public string CurrentPin { get; set; }
		public string NewPin { get; set; }
	}

}
