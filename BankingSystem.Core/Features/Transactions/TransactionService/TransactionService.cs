using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankingSystem.Core.Features.Transactions.TransactionRepository;

namespace BankingSystem.Core.Features.Transactions.TransactionService
{
	public class TransactionService : ITransactionService
	{
		private readonly ITransactionRepository _transactionRepository;

		public TransactionService(ITransactionRepository transactionRepository)
		{
			_transactionRepository = transactionRepository;
		}

		public async Task<int> AddTransactionAsync(Transaction transaction)
		{
			ValidateTransaction(transaction);


			// Calculate the transaction amount in the destination currency
			decimal destinationAmount = transaction.FromAmount * GetExchangeRate(transaction.FromAccountCurrency, transaction.ToAccountCurrency);

			// Calculate the fee (assuming Fee is a percentage)
			decimal fee = destinationAmount * 0.02m;

			// Deduct the fee
			destinationAmount -= fee;

			// Perform the transaction
			transaction.ToAmount = destinationAmount;
			transaction.Fee = fee;

			// Add the transaction using the repository
			return await _transactionRepository.AddTransactionAsync(transaction);
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
		private decimal GetExchangeRate(string fromCurrency, string toCurrency)
		{
			
			return 2.52m;
		}
		public async Task<List<Transaction>> GetTransactionsByAccountIdAsync(int accountId)
		{
			string query = "SELECT * FROM TransactionTable WHERE FromAccountId = @AccountId OR ToAccountId = @AccountId";
			var result = await _dataManager.Query<Transaction, dynamic>(query, new { AccountId = accountId });

			return result.ToList();
		}
	}
}
