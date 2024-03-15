using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.Core.Features.Atm.ChangePin.Requests;

namespace BankingSystem.Core.Features.Atm.ChangePin
{
	public interface IChangePinService
	{
		Task<bool> ChangePinAsync(string cardNumber, string currentPin, string newPin);
	}

}
