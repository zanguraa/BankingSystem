using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankingSystem.Core.Features.BankAccounts;
using BankingSystem.Core.Features.Transactions.CreateTransactions;
using BankingSystem.Core.Features.Transactions.Currency;
using BankingSystem.Core.Features.Transactions.TransactionsRepository;
using BankingSystem.Core.Shared.Exceptions;

namespace BankingSystem.Core.Features.Transactions.TransactionServices
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrencyConversionService _currencyConversionService;
        private readonly IBankAccountService _bankAccountService;
        private readonly IBankAccountRepository _bankAccountRepository;

        public TransactionService(
            IBankAccountService bankAccountService,
            ITransactionRepository transactionRepository,
            ICurrencyConversionService currencyConversionService,
            IBankAccountRepository bankAccountRepository)

        {
            _transactionRepository = transactionRepository;
            _currencyConversionService = currencyConversionService;
            _bankAccountService = bankAccountService;
            _bankAccountRepository = bankAccountRepository;
        }


        public async Task<TransactionResponse> TransferTransactionAsync(CreateTransactionRequest request)
        {
            if(string.IsNullOrEmpty(request.UserId))
            {
                throw new DomainException("User not found.");
            }

			await _bankAccountService.CheckAccountOwnershipAsync(request.FromAccountId, request.UserId);

			// Validate and fetch accounts
			var fromAccount = await _bankAccountRepository.GetAccountByIdAsync(request.FromAccountId);
            var toAccount = await _bankAccountRepository.GetAccountByIdAsync(request.ToAccountId);
            if (fromAccount == null || toAccount == null)
            {
                throw new ArgumentException("One or both account IDs are invalid.");
            }

            // Determine transaction type
            var transactionType = fromAccount.UserId == toAccount.UserId ? TransactionType.Internal : TransactionType.External;

            // Calculate fee and convert amount
            decimal transactionFee = CalculateTransactionFee(request.Amount, transactionType);
            decimal convertedAmount = _currencyConversionService.Convert(request.Amount, request.Currency, request.ToCurrency);

            // Check if the fromAccount has enough balance
            if (fromAccount.InitialAmount < (request.Amount + transactionFee))
            {
                throw new InvalidOperationException("Insufficient funds to complete this transaction.");
            }

            // ტრანზაქციის შექმნა
            var transaction = new Transaction
            {
                FromAccountId = request.FromAccountId,
                ToAccountId = request.ToAccountId,
                FromAccountCurrency = request.Currency,
                ToAccountCurrency = request.ToCurrency,
                FromAmount = request.Amount + transactionFee,
                ToAmount = convertedAmount,
                Fee = transactionFee,
                TransactionType = (int)transactionType,
                TransactionDate = DateTime.UtcNow
            };

            await _transactionRepository.UpdateAccountBalancesAsync(transaction);

            return new TransactionResponse
            {
                FromAccountId = transaction.FromAccountId,
                ToAccountId = transaction.ToAccountId,
                Amount = transaction.FromAmount,
                Currency = transaction.FromAccountCurrency,
                Fee = transaction.Fee,
                TransactionDate = transaction.TransactionDate
            };
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId)
        {
            return await _transactionRepository.GetTransactionsByAccountIdAsync(accountId);
        }

        private decimal CalculateTransactionFee(decimal amount, TransactionType transactionType)
        {
            decimal feePercentage = 0; // Initial fee percentage
            decimal fixedFee = 0; // Initial fixed fee

            if (transactionType == TransactionType.External)
            {
                feePercentage = 0.01M; // 1% fee for external transactions
                fixedFee = 0.5M; // Additional fixed fee for external transactions
            }

            // Calculate the total fee by applying the percentage to the amount and adding the fixed fee
            decimal totalFee = (amount * feePercentage) + fixedFee;
            return totalFee;
        }
    }
}
