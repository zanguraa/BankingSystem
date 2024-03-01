using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankingSystem.Core.Features.Transactions.TransactionRepository;

namespace BankingSystem.Core.Features.Transactions.TransactionService
{
	public class TransactionService : ITransactionService, ITransactionService
	{
		private readonly ITransactionRepository _transactionRepository;

		public TransactionService(ITransactionRepository transactionRepository)
		{
			_transactionRepository = transactionRepository;
		}

		public async Task<int> AddTransactionAsync(Transaction transaction)
		{

			ValidateTransaction(transaction);

			CalculateFees(transaction);

			int newTransactionId = await _transactionRepository.AddTransactionAsync(transaction);
			return newTransactionId;

		}

		private void CalculateFees(Transaction transaction)
		{
			// Example fee calculation: 2% withdrawal fee
			double withdrawalFeePercentage = 0.02;
			double withdrawalFee = transaction.FromAmount * withdrawalFeePercentage;

			// Update transaction amounts and fee
			transaction.Fee = withdrawalFee;
			transaction.FromAmount -= withdrawalFee;
		}

		public async Task<List<Transaction>> GetTransactionsByAccountIdAsync(int accountId)
		{
			string query = "SELECT * FROM TransactionTable WHERE FromAccountId = @AccountId OR ToAccountId = @AccountId";
			var result = await _dataManager.Query<Transaction, dynamic>(query, new { AccountId = accountId });

			return result.ToList();
		}
	}
}
