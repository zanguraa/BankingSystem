using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankingSystem.Core.Features.BankAccounts;
using BankingSystem.Core.Features.Transactions.CreateTransaction;
using BankingSystem.Core.Features.Transactions.TransactionRepository;

namespace BankingSystem.Core.Features.Transactions.TransactionService
{
	public class TransactionService : ITransactionService
	{
		private readonly ITransactionRepository _transactionRepository;
		private readonly ICurrencyConversionService _currencyConversionService;

		public TransactionService(ITransactionRepository transactionRepository, ICurrencyConversionService currencyConversionService)
		{
			_transactionRepository = transactionRepository;
			_currencyConversionService = currencyConversionService;
		}

		public Task<int> AddTransactionAsync(Transaction transaction)
		{
			throw new NotImplementedException();
		}

		public async Task<TransactionResponse> CreateTransactionAsync(CreateTransactionRequest request)
		{
			// Validate accounts first
			bool fromAccountIsValid = await _bankAccountService.ValidateAccountAsync(request.FromAccountId);
			bool toAccountIsValid = await _bankAccountService.ValidateAccountAsync(request.ToAccountId);

			if (!fromAccountIsValid || !toAccountIsValid)
			{
				throw new ArgumentException("One or both account IDs are invalid.");
			}

			// Calculate fees
			decimal transactionFee = CalculateTransactionFee(request.Amount);

			// Convert currency if necessary
			decimal convertedAmount = request.Amount;
			if (request.Currency != "GEL") // Check if currency is not GEL
			{
				convertedAmount = await _currencyConversionService.ConvertAsync(request.Amount, request.Currency, "GEL");
			}

			// Create the transaction
			var transaction = new Transaction
			{
				FromAccountId = request.FromAccountId,
				ToAccountId = request.ToAccountId,
				FromAccountCurrency = request.Currency,
				ToAccountCurrency = "GEL",
				FromAmount = request.Amount,
				ToAmount = convertedAmount - transactionFee,
				Fee = transactionFee,
				TransactionDate = DateTime.UtcNow
			};

			await _transactionRepository.CreateTransactionAsync(transaction);

			// Convert to response DTO
			return new TransactionResponse
			{
				TransactionId = transaction.TransactionId,
				FromAccountId = transaction.FromAccountId,
				ToAccountId = transaction.ToAccountId,
				Amount = transaction.FromAmount,
				Currency = transaction.FromAccountCurrency,
				Fee = transaction.Fee,
				TransactionDate = transaction.TransactionDate
			};
		}

		public Task<List<Transaction>> GetTransactionsByAccountIdAsync(int accountId)
		{
			throw new NotImplementedException();
		}

		private decimal CalculateTransactionFee(decimal amount)
		{
			return amount * 0.02m; // 2% transaction fee
		}
	}
}
