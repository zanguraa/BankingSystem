using BankingSystem.Core.Features.Atm.CardAuthorizations.Models.Requests;
using BankingSystem.Core.Features.Atm.ChangePin.Models.Requests;
using BankingSystem.Core.Features.Atm.ViewBalance.Models.Response;
using BankingSystem.Core.Features.Atm.WithdrawMoney.Models.Requests;
using BankingSystem.Core.Features.BankAccounts.AddFunds.Models.Requests;
using BankingSystem.Core.Features.BankAccounts.CreateAccount.Models.Requests;
using BankingSystem.Core.Features.Transactions.Shared.Models.Requests;
using BankingSystem.Core.Features.Cards.CreateCard.Models.Requests;
using BankingSystem.Core.Features.Users.AuthorizeUser.Requests;
using BankingSystem.Core.Features.Users.CreateUser.Models.Requests;

namespace BankingSystem.Test.Factory
{
    public class ModelFactory
	{
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
				Pin = "1234" 
			};

			options?.Invoke(request);

			return request;
		}
		public static ChangePinRequest GetChangePinRequest(Action<ChangePinRequest> customizer = null)
		{
			var request = new ChangePinRequest
			{
				CardNumber = "6410341801621247", 
				CurrentPin = "1234",
				NewPin = "5678"
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
		
		public static RegisterUserRequest GetRegisterUserRequest(Action<RegisterUserRequest> customize = null)
		{
			var request = new RegisterUserRequest
			{
				Email = "test@example.com",
				Password = "securePassword123",
				PasswordConfirmed = "securePassword123", // Ensure this matches the password if required
				FirstName = "John",
				LastName = "Doe",
				PhoneNumber = "1234567890",
				PersonalId = "12345678901",
				BirthdayDate = DateTime.Today.AddYears(-25)
			};

			customize?.Invoke(request);
			return request;
		}


		public static BalanceResponse CreateBalanceResponse()
		{
			return new BalanceResponse
			{
				UserId = "1",
				InitialAmount = 1000,
				Currency = "USD"
			};
		}
		public static LoginRequest GetLoginRequest(Action<LoginRequest> customize = null)
		{
			var request = new LoginRequest
			{
				Email = "user@example.com",
				Password = "SecurePassword123!"
			};

			customize?.Invoke(request);
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

		public static CreateTransactionRequest GetCreateTransactionRequest(Action<CreateTransactionRequest> customize)
		{
			var request = new CreateTransactionRequest
			{
				FromAccountId = 1,
				ToAccountId = 2,
				Amount = 100.00m,
				Currency = "USD",
				ToCurrency = "EUR",
				UserId = "user123"
			};

			customize?.Invoke(request);
			return request;
		}

		public static WithdrawAmountCurrencyRequest GetWithdrawAmountCurrencyRequest(Action<WithdrawAmountCurrencyRequest> customize = null)
		{
			var request = new WithdrawAmountCurrencyRequest
			{
				Amount = 100,
				Currency = "USD"
			};

			customize?.Invoke(request);
			return request;
		}
	
	}
}
