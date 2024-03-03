using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Transactions;
using BankingSystem.Core.Features.Transactions.TransactionsRepository;
using Dapper;

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

    private async Task UpdateAccountBalancesAsync(Transaction transaction)
    {
        // Assume accounts have columns: AccountId, InitialAmount, Currency
        // Adjust the queries based on your actual schema and logic for currency conversion if necessary

        // Update FromAccountId balance
        string updateFromAccountQuery = @"
            UPDATE BankAccounts
            SET InitialAmount = InitialAmount - @FromAmount - @Fee
            WHERE Id = @FromAccountId";

        await _dataManager.Execute(updateFromAccountQuery, new { transaction.FromAccountId, transaction.FromAmount, transaction.Fee });

        // Update ToAccountId balance
        string updateToAccountQuery = @"
            UPDATE BankAccounts
            SET InitialAmount = InitialAmount + @ToAmount
            WHERE Id = @ToAccountId";

        await _dataManager.Execute(updateToAccountQuery, new { transaction.ToAccountId, transaction.ToAmount });
    }
}