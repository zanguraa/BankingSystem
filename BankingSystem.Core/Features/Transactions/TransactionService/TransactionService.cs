using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Transactions.TransactionRepository;

namespace BankingSystem.Core.Features.Transactions.TransactionService
{
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
                INSERT INTO TransactionTable (FromAccountId, ToAccountId, FromAccountCurrency, ToAccountCurrency,
                                              FromAmount, ToAmount, Fee, TransactionDate, TransactionType, AddFunds)
                VALUES (@FromAccountId, @ToAccountId, @FromAccountCurrency, @ToAccountCurrency,
                        @FromAmount, @ToAmount, @Fee, @TransactionDate, @TransactionType, @AddFunds);";

			var result = await _dataManager.Execute(query, new
			{
				transaction.FromAccountId,
				transaction.ToAccountId,
				transaction.FromAccountCurrency,
				transaction.ToAccountCurrency,
				transaction.FromAmount,
				transaction.ToAmount,
				transaction.Fee,
				transaction.TransactionDate,
				transaction.TransactionType,
				transaction.AddFunds
			});

			if (result == 0)
			{
				throw new Exception("Failed to add transaction");
			}

			// Implement logic to return the ID of the newly created transaction
			return 0; // Replace with the actual ID retrieval logic
		}

		public async Task<List<Transaction>> GetTransactionsByAccountIdAsync(int accountId)
		{
			string query = "SELECT * FROM TransactionTable WHERE FromAccountId = @AccountId OR ToAccountId = @AccountId";
			var result = await _dataManager.Query<Transaction, dynamic>(query, new { AccountId = accountId });
			return result.ToList();
		}
	}

}
