using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankingSystem.Core.Features.BankAccounts;
using BankingSystem.Core.Features.Transactions.CreateTransactions;
using BankingSystem.Core.Features.Transactions.Currency;
using BankingSystem.Core.Features.Transactions.TransactionsRepository;

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


        public async Task<TransactionResponse> CreateTransactionAsync(CreateTransactionRequest request)
        {
            // ექაუნთების ვალიდაცია
            // გვჭირდება აიდიების მიხედვით შემოწმება, თუ იუზერ აიდიები ერთნაირია, მაშინ ინტერნალ ტრანზაქციაა, თუ არა და მაშინ ექსტერნალ.

            // Get accounts asynchronously
            var fromAccount = await _bankAccountRepository.GetAccountByIdAsync(request.FromAccountId);
            var toAccount = await _bankAccountRepository.GetAccountByIdAsync(request.ToAccountId);

            // Check if accounts exist
            if (fromAccount == null || toAccount == null)
            {
                throw new ArgumentException("One or both account IDs are invalid.");
            }

            var fromAccountUser = fromAccount.UserId;
            var toAccountUser = toAccount.UserId;
            // Determine transaction type based on user IDs
            var transactionType = fromAccountUser == toAccountUser ? TransactionType.Internal : TransactionType.External;



            // fee კალკულაცია
            decimal transactionFee = CalculateTransactionFee(request.Amount, transactionType);

            // currency კონვერტაცია

            decimal convertedAmount = _currencyConversionService.Convert(request.Amount, request.Currency, request.ToCurrency);

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

            await _transactionRepository.CreateTransactionAsync(transaction);


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

        private decimal CalculateTransactionFee(decimal amount, TransactionType transactionType)
        {
            // Calculate fee based on transaction type
            var fee = transactionType == TransactionType.External ? 0.01M + 0.5m: 0;
            return amount * fee; // 2% transaction fee
        }
    }
}
