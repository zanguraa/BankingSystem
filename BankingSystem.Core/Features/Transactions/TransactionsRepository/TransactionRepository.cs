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

	public async Task<Transaction> GetTransactionByIdAsync(int transactionId)
	{
		string query = "SELECT * FROM Transactions WHERE TransactionId = @TransactionId";
		var transaction = await _dataManager.Query<Transaction, dynamic>(query, new { TransactionId = transactionId });
		return transaction.FirstOrDefault();
	}
}