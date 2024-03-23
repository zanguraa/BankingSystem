using BankingSystem.Core.Features.BankAccounts.BankAccountRepositories;
using BankingSystem.Core.Features.BankAccounts.BankAccountsServices;
using BankingSystem.Core.Features.Transactions.CreateTransactions;
using BankingSystem.Core.Features.Transactions.Currency;
using BankingSystem.Core.Features.Transactions.TransactionsRepositories;

namespace BankingSystem.Core.Features.Transactions.TransactionServices
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId);
        Task<TransactionResponse> ProcessInternalTransactionAsync(CreateTransactionRequest request);
        Task<TransactionResponse> TransferTransactionAsync(CreateTransactionRequest request);
    }

    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrencyConversionService _currencyConversionService;
        private readonly IBankAccountService _bankAccountService;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ITransactionServiceValidator _transactionServiceValidator;

        public TransactionService(
            IBankAccountService bankAccountService,
            ITransactionRepository transactionRepository,
            ICurrencyConversionService currencyConversionService,
            IBankAccountRepository bankAccountRepository,
            ITransactionServiceValidator transactionServiceValidator)

        {
            _transactionRepository = transactionRepository;
            _currencyConversionService = currencyConversionService;
            _bankAccountService = bankAccountService;
            _bankAccountRepository = bankAccountRepository;
            _transactionServiceValidator = transactionServiceValidator;
        }


        public async Task<TransactionResponse> TransferTransactionAsync(CreateTransactionRequest request)
        {
            using var semaphore = new SemaphoreSlim(1, 1);

            await semaphore.WaitAsync();

            await Task.Delay(10000);

            await _transactionServiceValidator.ValidateCreateTransactionRequest(request);
            await _bankAccountService.CheckAccountOwnershipAsync(request.FromAccountId, request.UserId);

            var fromAccount = await _bankAccountRepository.GetAccountByIdAsync(request.FromAccountId);
            var toAccount = await _bankAccountRepository.GetAccountByIdAsync(request.ToAccountId);
            if (fromAccount == null || toAccount == null)
            {
                throw new ArgumentException("One or both account IDs are invalid.");
            }

            var transactionType = fromAccount.UserId == toAccount.UserId ? TransactionType.Internal : TransactionType.External;

            decimal transactionFee = CalculateTransactionFee(request.Amount, transactionType);
            decimal convertedAmount = _currencyConversionService.Convert(request.Amount, request.Currency, request.ToCurrency);

            if (fromAccount.InitialAmount < (request.Amount + transactionFee))
            {
                throw new InvalidOperationException("Insufficient funds to complete this transaction.");
            }

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

            semaphore.Release();

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

        public async Task<TransactionResponse> ProcessInternalTransactionAsync(CreateTransactionRequest request)
        {
            using var semaphore = new SemaphoreSlim(1, 1);

            await _transactionServiceValidator.ValidateCreateTransactionRequest(request);
            await _bankAccountService.CheckAccountOwnershipAsync(request.FromAccountId, request.UserId);

            await semaphore.WaitAsync();

            var fromAccount = await _bankAccountRepository.GetAccountByIdAsync(request.FromAccountId);
            var toAccount = await _bankAccountRepository.GetAccountByIdAsync(request.ToAccountId);

            if (fromAccount == null || toAccount == null || fromAccount.UserId != toAccount.UserId)
            {
                throw new ArgumentException("Invalid accounts for internal transaction.");
            }

            if (fromAccount.InitialAmount < request.Amount)
            {
                throw new InvalidOperationException("Insufficient funds.");
            }

            var transaction = new Transaction
            {
                FromAccountId = request.FromAccountId,
                ToAccountId = request.ToAccountId,
                FromAccountCurrency = request.Currency,
                ToAccountCurrency = request.ToCurrency,
                FromAmount = request.Amount,
                ToAmount = request.Amount, // Assuming no conversion for internal transactions
                Fee = 0,
                TransactionType = (int)TransactionType.Internal,
                TransactionDate = DateTime.UtcNow
            };

            await _transactionRepository.UpdateAccountBalancesAsync(transaction);

            semaphore.Release();

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
            decimal feePercentage = 0;
            decimal fixedFee = 0;

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
