using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Transactions;
using BankingSystem.Core.Features.Transactions.TransactionsRepository;
using Dapper;
using Microsoft.EntityFrameworkCore;

public class TransactionRepository : ITransactionRepository
{
	private readonly IDataManager _dataManager;

	public TransactionRepository(IDataManager dataManager)
	{
		_dataManager = dataManager;
	}

    public async Task<int> AddTransactionAsync(Transaction transaction)
    {
        string query = @"
            INSERT INTO Transactions (FromAccountId, ToAccountId, FromAccountCurrency, ToAccountCurrency, FromAmount, ToAmount, TransactionDate, TransactionType, Fee)
            VALUES (@FromAccountId, @ToAccountId, @FromAccountCurrency, @ToAccountCurrency, @FromAmount, @ToAmount, @TransactionDate, @TransactionType, @Fee);";

        var rows = await _dataManager.Execute(query, transaction);
        if (rows > 0)
        {
            await UpdateAccountBalancesAsync(transaction);
        }
        return rows;
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId)
	{
		string query = @"
            SELECT * FROM Transactions
            WHERE FromAccountId = @AccountId OR ToAccountId = @AccountId";

		var transactions = await _dataManager.Query<Transaction, dynamic>(query, new { AccountId = accountId });
		return transactions;
	}

    public async Task<bool> CheckAccountOwnershipAsync(int accountId, string userId)
    {
        var sql = @"SELECT COUNT(1) FROM [BankingSystem_db].[dbo].[BankAccounts] WHERE Id = @AccountId AND UserId = @UserId";
        var parameters = new { AccountId = accountId, UserId = userId };
        var count = await _dataManager.Query<int, dynamic>(sql, parameters);
        return count.FirstOrDefault() > 0;
    }

    private async Task UpdateAccountBalancesAsync(Transaction transaction)
    {
       

        // Update FromAccountId balance
        string updateFromAccountQuery = @"
            UPDATE BankAccounts
            SET InitialAmount = InitialAmount - @FromAmount 
            WHERE Id = @FromAccountId";

        await _dataManager.Execute(updateFromAccountQuery, new { transaction.FromAccountId, transaction.FromAmount  });

        // Update ToAccountId balance
        string updateToAccountQuery = @"
            UPDATE BankAccounts
            SET InitialAmount = InitialAmount + @ToAmount
            WHERE Id = @ToAccountId";

        await _dataManager.Execute(updateToAccountQuery, new { transaction.ToAccountId, transaction.ToAmount });
    }
}