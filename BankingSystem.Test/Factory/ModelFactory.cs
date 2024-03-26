using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.Core.Features.Transactions.CreateTransactions;
using BankingSystem.Core.Features.Atm.WithdrawMoney.Requests;

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
	}
}
