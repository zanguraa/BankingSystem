using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Atm.WithdrawMoney;
using BankingSystem.Core.Features.Transactions;
using BankingSystem.Core.Features.Transactions.TransactionsRepository;
using Dapper;
using Microsoft.EntityFrameworkCore;

public class WithdrawMoneyRepository : IWithdrawMoneyRepository
{
	private readonly IDataManager _dataManager;

	public WithdrawMoneyRepository(IDataManager dataManager)
	{
		_dataManager = dataManager;
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
}