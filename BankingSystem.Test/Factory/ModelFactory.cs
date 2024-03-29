using BankingSystem.Core.Features.Atm.CardAuthorizations.Models.Requests;
using BankingSystem.Core.Features.Atm.ChangePin.Models.Requests;
using BankingSystem.Core.Features.BankAccounts.AddFunds.Models.Requests;
using BankingSystem.Core.Features.BankAccounts.CreateAccount.Models.Requests;
using BankingSystem.Core.Features.Transactions.CreateTransactions.Models.Requests;

namespace BankingSystem.Test.Factory
{
    public class ModelFactory
	{
		public static CreateTransactionRequest GetCreateTransactionRequest(Action<CreateTransactionRequest> options = null)
		{
			CreateTransactionRequest request = new()
			{
				UserId = "1",
				FromAccountId = 1,
				ToAccountId= 2,
				Amount= 1,
				Currency="GEL",
				ToCurrency="GEL"
			};

			options?.Invoke(request);

			return request;
		}
		public static WithdrawRequestWithCardNumber GetWithdrawMoneyRequest(Action<WithdrawRequestWithCardNumber> options = null)
		{
			WithdrawRequestWithCardNumber request = new()
			{
				CardNumber = "1234567890123456",
				Amount = 50,
				Currency = "USD"
			};

			options?.Invoke(request);
			return request;
		}
		public static CardAuthorizationRequest GetCardAuthorizationRequest(Action<CardAuthorizationRequest> options = null)
		{
			var request = new CardAuthorizationRequest
			{
				CardNumber = "1234567890123456", // Assume a valid card number
				Pin = 1234 // Assume a valid pin
			};

			options?.Invoke(request);

			return request;
		}
		public static ChangePinRequest GetChangePinRequest(Action<ChangePinRequest> customizer = null)
		{
			var request = new ChangePinRequest
			{
				CardNumber = "1111222233334444", // Valid card number
				CurrentPin = 1234,
				NewPin = 5678
			};

			customizer?.Invoke(request);

			return request;
		}
		public static AddFundsRequest GetAddFundsRequest(Action<AddFundsRequest> customizer = null)
		{
			var request = new AddFundsRequest
			{
				BankAccountId = 1, // Default to a valid BankAccountId
				Amount = 100.00M // Default to a valid amount
			};

			customizer?.Invoke(request);

			return request;
		}
		public static CreateBankAccountRequest GetCreateBankAccountRequest(Action<CreateBankAccountRequest> setup = null)
		{
			var request = new CreateBankAccountRequest
			{
				UserId = 1 // Set a default valid user id for testing
			};

			setup?.Invoke(request);

			return request;
		}
	}
}
