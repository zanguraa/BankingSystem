using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Atm.CardAuthorization
{
	public class CardAuthorizationService : ICardAuthorizationService, ICardAuthorizationService
	{
		private readonly IBankAccountRepository _bankAccountRepository;

		public CardAuthorizationService(IBankAccountRepository bankAccountRepository)
		{
			_bankAccountRepository = bankAccountRepository;
		}

		public async Task<bool> AuthorizeCardAsync(string cardNumber, string pinCode)
		{
			// Retrieve the bank account using the card number
			var bankAccount = await _bankAccountRepository.GetBankAccountByCardNumberAsync(cardNumber);

			// Check if the bank account exists and the card has not expired
			if (bankAccount == null || bankAccount.ExpiryDate < DateTime.UtcNow)
			{
				return false; // Card not found or expired
			}

			// Verify the PIN code, ensure you are using a secure method for storing and checking PIN codes
			// In a real-world scenario, the PIN would be hashed and you would compare the hashes here
			return bankAccount.PinCode == pinCode;
		}
	}
}