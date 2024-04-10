
using BankingSystem.Core.Features.Transactions.Shared.Models.Requests;
using BankingSystem.Core.Features.Transactions.Shared.Models.Response;
using BankingSystem.Core.Features.Transactions.Shared;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Models;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Core.Shared.Currency;

namespace BankingSystem.Core.Features.Transactions.InternalTransaction
{
    public interface IInternalTransactionService
    {
        Task<CreateTransactionResponse> ProcessInternalTransactionAsync(CreateTransactionRequest request);
    }

    public class InternalTransactionService : IInternalTransactionService
    {
        private readonly ICreateTransactionRepository _createtransactionRepository;
        private readonly ICurrencyConversionService _currencyConversionService;
        private readonly ICreateTransactionService _createTransactionService;
        private readonly ITransactionServiceValidator _transactionServiceValidator;
        private readonly ISeqLogger _logger;
        public InternalTransactionService(
            ICreateTransactionRepository transactionRepository,
            ICurrencyConversionService currencyConversionService,
            ICreateTransactionService createTransactionService,
            ISeqLogger logger,
            ITransactionServiceValidator transactionServiceValidator)
        {
            _createtransactionRepository = transactionRepository;
            _currencyConversionService = currencyConversionService;
            _createTransactionService = createTransactionService;
            _logger = logger;
            _transactionServiceValidator = transactionServiceValidator;
        }

        public async Task<CreateTransactionResponse> ProcessInternalTransactionAsync(CreateTransactionRequest request)
        {
            using var semaphore = new SemaphoreSlim(1, 1);

            await _transactionServiceValidator.ValidateCreateTransactionRequestAsync(request);
            await _createTransactionService.CheckAccountOwnershipAsync(request.FromAccountId, request.UserId);

            await semaphore.WaitAsync();

            var fromAccount = await _createtransactionRepository.GetAccountByIdAsync(request.FromAccountId);
            var toAccount = await _createtransactionRepository.GetAccountByIdAsync(request.ToAccountId);

            if (fromAccount.UserId.ToString() != request.UserId || toAccount.UserId.ToString() != request.UserId)
            {
                throw new InvalidAccountException("Both accounts must belong to the user with id: {userId}. account Id's: {fromAccount} - {toAccount}. only {fromAccount} is belong to {userId}", request.UserId, fromAccount.Id, toAccount.Id);
            }

            decimal convertedAmount = _currencyConversionService.Convert(request.Amount, request.Currency, request.ToCurrency);

            if (fromAccount == null || toAccount == null || fromAccount.UserId != toAccount.UserId)
            {
                throw new ArgumentException("Invalid accounts for internal transaction.");
            }

            if (fromAccount.InitialAmount < request.Amount)
            {
                throw new InvalidTransactionException("Insufficient funds for Iban:{Iban}.", fromAccount.Iban);
            }

            var transaction = new Transaction
            {
                FromAccountId = request.FromAccountId,
                ToAccountId = request.ToAccountId,
                FromAccountCurrency = request.Currency,
                ToAccountCurrency = request.ToCurrency,
                FromAmount = request.Amount,
                ToAmount = convertedAmount,
                Fee = 0,
                TransactionType = (int)TransactionType.Internal,
                TransactionDate = DateTime.UtcNow
            };

            await _createtransactionRepository.ProcessBankTransactionAsync(transaction);

            semaphore.Release();

            _logger.LogInfo("Internal Transaction! fromAccount: {fromAccount}, was sent money: {amount}-{currency} toAccountId {toAccountId}", request.FromAccountId, request.Amount, request.ToCurrency, request.ToAccountId);

            return new CreateTransactionResponse
            {
                FromAccountId = transaction.FromAccountId,
                ToAccountId = transaction.ToAccountId,
                Amount = transaction.FromAmount,
                Currency = transaction.FromAccountCurrency,
                Fee = transaction.Fee,
                TransactionDate = transaction.TransactionDate
            };

        }

    }
}
