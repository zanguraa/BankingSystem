using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Transactions;
using BankingSystem.Core.Features.Transactions.TransactionRepository;
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
            INSERT INTO Transactions (FromAccountId, ToAccountId, FromAccountCurrency, ToAccountCurrency, FromAmount, ToAmount, Fee, TransactionDate)
            VALUES (@FromAccountId, @ToAccountId, @FromAccountCurrency, @ToAccountCurrency, @FromAmount, @ToAmount, @Fee, @TransactionDate);
            SELECT CAST(SCOPE_IDENTITY() as int);";

		var transactionId = await _dataManager.Query<int, Transaction>(query, transaction);
		return transactionId.FirstOrDefault();
	}

	public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
	{
		var transactionId = await AddTransactionAsync(transaction);
		transaction.TransactionId = transactionId;
		return transaction;
	}

	public async Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId)
	{
		string query = @"
            SELECT * FROM Transactions
            WHERE FromAccountId = @AccountId OR ToAccountId = @AccountId";

		var transactions = await _dataManager.Query<Transaction, dynamic>(query, new { AccountId = accountId });
		return transactions;
	}

	public async Task<bool> UpdateTransactionAsync(Transaction transaction)
	{
		string query = @"
            UPDATE Transactions
            SET FromAccountId = @FromAccountId, ToAccountId = @ToAccountId, FromAccountCurrency = @FromAccountCurrency, ToAccountCurrency = @ToAccountCurrency, FromAmount = @FromAmount, ToAmount = @ToAmount, Fee = @Fee, TransactionDate = @TransactionDate
            WHERE TransactionId = @TransactionId";

		var result = await _dataManager.Execute(query, transaction);
		return result > 0;
	}

	public async Task<bool> DeleteTransactionAsync(int transactionId)
	{
		string query = "DELETE FROM Transactions WHERE TransactionId = @TransactionId";
		var result = await _dataManager.Execute(query, new { TransactionId = transactionId });
		return result > 0;
	}

	public async Task<Transaction> GetTransactionByIdAsync(int transactionId)
	{
		string query = "SELECT * FROM Transactions WHERE TransactionId = @TransactionId";
		var transaction = await _dataManager.Query<Transaction, dynamic>(query, new { TransactionId = transactionId });
		return transaction.FirstOrDefault();
	}

	Task<List<Transaction>> ITransactionRepository.GetTransactionsByAccountIdAsync(int accountId)
	{
		throw new NotImplementedException();
	}
}