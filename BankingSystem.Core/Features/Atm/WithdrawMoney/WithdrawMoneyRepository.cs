using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using BankingSystem.Core.Data;

namespace BankingSystem.Core.Features.Atm.WithdrawMoney
{
	public class WithdrawMoneyRepository : IWithdrawMoneyRepository
	{
		private readonly IDataManager _dataManager;

		public WithdrawMoneyRepository(IDataManager dataManager)
		{
			_dataManager = dataManager;
		}
		public Task<bool> RecordTransactionAsync(Transaction transaction)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> WithdrawAsync(int accountId, decimal amount, string currency)
		{
			// Prepare SQL commands for the withdrawal operation
			SqlCommandRequest decreaseAccountBalanceCommand = new()
			{
				Query = @"
                UPDATE BankAccounts
                SET InitialAmount = InitialAmount - @Amount
                WHERE Id = @AccountId AND Currency = @Currency",
				Params = new { AccountId = accountId, Amount = amount, Currency = currency }
			};

			SqlCommandRequest insertWithdrawalTransactionCommand = new()
			{
				Query = @"
            INSERT INTO Transactions (FromAccountId, ToAccountId, FromAccountCurrency, ToAccountCurrency, FromAmount, ToAmount, TransactionDate, TransactionType, Fee)
            VALUES (@FromAccountId, NULL, @Currency, NULL, @Amount, 0, GETDATE(), 'Withdrawal', 0);",
				Params = new { FromAccountId = accountId, Currency = currency, Amount = amount }
			};

			// Execute the SQL commands within a transaction
			var sqlCommandRequests = new List<SqlCommandRequest>
		{
			decreaseAccountBalanceCommand,
			insertWithdrawalTransactionCommand
		};

			bool success = await _dataManager.ExecuteWithTransaction(sqlCommandRequests);

			if (!success)
			{
				// Consider more specific exception types based on the failure reason
				throw new Exception("Failed to complete the withdrawal operation.");
			}

			return success;
		}

		public Task<bool> WithdrawAsync(string accountNumber, decimal amount)
		{
			throw new NotImplementedException();
		}
	}
}
