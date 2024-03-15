using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Atm.ChangePin.Requests
{
	public class ChangePinResponse
	{
		public bool Success { get; set; }
		public string Message { get; set; }
	}

}
