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

        public Task<int> AddTransactionAsync(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public async Task<TransactionResponse> CreateTransactionAsync(CreateTransactionRequest request)
        {
            // ექაუნთების ვალიდაცია
            // გვჭირდება აიდიების მიხედვით შემოწმება, თუ იუზერ აიდიები ერთნაირია, მაშინ ინტერნალ ტრანზაქციაა, თუ არა და მაშინ ექსტერნალ.

            // Get accounts asynchronously
            var fromAccount = await _bankAccountRepository.GetAccountByIdAsync(request.FromAccountId);
            var toAccount = await _bankAccountRepository.GetAccountByIdAsync(request.ToAccountId);

            // Check if accounts exist
            if (!fromAccount || !toAccount)
            {
                throw new ArgumentException("One or both account IDs are invalid.");
            }

            // Determine transaction type based on user IDs
            var transactionType = fromAccount == toAccount ? TransactionType.Internal : TransactionType.External;

            // Calculate fee based on transaction type
            var fee = transactionType == TransactionType.External ? 0.01M : 0.0M;




            bool fromAccountIsValid = await _bankAccountService.ValidateAccountAsync(request.FromAccountId);
            bool toAccountIsValid = await _bankAccountService.ValidateAccountAsync(request.ToAccountId);

            if (!fromAccountIsValid || !toAccountIsValid)
            {
                throw new ArgumentException("One or both account IDs are invalid.");
            }

            // fee კალკულაცია
            decimal transactionFee = CalculateTransactionFee(request.Amount);

            // currency კონვერტაცია
            decimal convertedAmount = request.Amount;
            if (request.Currency != "GEL") // Check if currency is not GEL
            {
                convertedAmount = await _currencyConversionService.ConvertAsync(request.Amount, request.Currency, "GEL");
            }

            // ტრანზაქციის შექმნა
            var transaction = new Transaction
            {
                FromAccountId = request.FromAccountId,
                ToAccountId = request.ToAccountId,
                FromAccountCurrency = request.Currency,
                ToAccountCurrency = "GEL",
                FromAmount = request.Amount,
                ToAmount = convertedAmount - transactionFee,
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
