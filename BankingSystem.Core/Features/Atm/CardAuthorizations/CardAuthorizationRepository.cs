using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.Core.Data;

namespace BankingSystem.Core.Features.Atm.CardAuthorization
{
	public class CardAuthorizationRepository : ICardAuthorizationRepository
	{
		private readonly IDataManager _dataManager;

		public CardAuthorizationRepository(IDataManager dataManager)
		{
			_dataManager = dataManager;
		}

		public async Task<BankAccount> GetBankAccountByCardNumberAsync(string cardNumber)
		{
			var query = "SELECT * FROM BankAccounts WHERE CardNumber = @CardNumber";
			var result = await _dataManager.Query<BankAccount, dynamic>(query, new { CardNumber = cardNumber });
			return result.FirstOrDefault();
		}
		public async Task<bool> UpdatePinCodeAsync(string cardNumber, string newPinHash)
		{
			var query = "UPDATE BankAccounts SET PinCode = @NewPinHash WHERE CardNumber = @CardNumber";
			var result = await _dataManager.Execute(query, new { CardNumber = cardNumber, NewPinHash = newPinHash });
			return result > 0;
		}
		public async Task LogAuthorizationAttemptAsync(string cardNumber, bool isSuccess)
		{
			var query = "INSERT INTO AuthorizationAttempts (CardNumber, IsSuccess, AttemptDate) VALUES (@CardNumber, @IsSuccess, @AttemptDate)";
			await _dataManager.Execute(query, new { CardNumber = cardNumber, IsSuccess = isSuccess, AttemptDate = DateTime.UtcNow });
		}
		public async Task<bool> IsCardActivatedAsync(string cardNumber)
		{
			var query = "SELECT IsActive FROM BankAccounts WHERE CardNumber = @CardNumber";
			var result = await _dataManager.Query<bool,dynamic>(query, new { CardNumber = cardNumber });
			return result.FirstOrDefault();
		}
	}
}
