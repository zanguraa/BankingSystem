using BankingSystem.Core.Features.Atm.CardAuthorizations.Models.Requests;
using BankingSystem.Core.Features.Atm.ChangePin.Models.Requests;
using BankingSystem.Core.Features.Atm.WithdrawMoney.Models.Requests;
using BankingSystem.Core.Features.BankAccounts.AddFunds.Models.Requests;
using BankingSystem.Core.Features.BankAccounts.CreateAccount.Models.Requests;
using BankingSystem.Core.Features.Cards.CreateCard.Models.Requests;
using BankingSystem.Core.Features.Transactions.CreateTransactions.Models.Requests;
using BankingSystem.Core.Features.Users.CreateUser.Requests;

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
				ToAccountId = 2,
				Amount = 1,
				Currency = "GEL",
				ToCurrency = "GEL"
			};

			options?.Invoke(request);

			return request;
		}

		public static WithdrawAmountCurrencyRequest GetWithdrawMoneyRequest(Action<WithdrawAmountCurrencyRequest> options = null)
		{
			var request = new WithdrawAmountCurrencyRequest
			{
				CardNumber = "6410341801621247", 
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
				CardNumber = "6410341801621247", 
				Pin = 1234 
			};

			options?.Invoke(request);

			return request;
		}
		public static ChangePinRequest GetChangePinRequest(Action<ChangePinRequest> customizer = null)
		{
			var request = new ChangePinRequest
			{
				CardNumber = "6410341801621247", 
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
				BankAccountId = 1, 
				Amount = 100.00M 
			};

			customizer?.Invoke(request);

			return request;
		}
		public static CreateBankAccountRequest GetCreateBankAccountRequest(Action<CreateBankAccountRequest> customize = null)
		{
			var request = new CreateBankAccountRequest
			{
				UserId = 1, 
			};

			customize?.Invoke(request);

			return request;
		}
		public static CreateCardRequest GetCreateCardRequest(Action<CreateCardRequest> customize = null)
		{
			var request = new CreateCardRequest
			{
				UserId = 1, 
				AccountId = 1,
			};

			customize?.Invoke(request);
			return request;
		}
		public static RegisterUserRequest GetRegisterUserRequest(Action<RegisterUserRequest> customizeRequest = null)
		{
			var request = new RegisterUserRequest
			{
				Email = "test@example.com",
				FirstName = "John",
				LastName = "Doe",
				PersonalId = "12345678901",
				PhoneNumber = "123456789",
				BirthdayDate = new DateTime(1990, 1, 1),
				Password = "Password123!"
			};

			customizeRequest?.Invoke(request);

			return request;
		}
	}
}
