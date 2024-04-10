using BankingSystem.Core.Features.Transactions.Shared.Models.Requests;
using BankingSystem.Core.Features.Transactions.Shared.Models.Response;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Currency;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Core.Shared.Models;

namespace BankingSystem.Core.Features.Transactions.Shared;

public interface ICreateTransactionService
{
    Task<CreateTransactionResponse> ProcessWithdrawalTransactionAsync(CreateTransactionRequest request);
    Task<bool> CheckAccountOwnershipAsync(int accountId, string userId);
    decimal CalculateTransactionFee(decimal amount, TransactionType transactionType);

}

public class CreateTransactionService : ICreateTransactionService
{
    private readonly ICreateTransactionRepository _createtransactionRepository;
    private readonly ICurrencyConversionService _currencyConversionService;
    private readonly ITransactionServiceValidator _transactionServiceValidator;
    private readonly ISeqLogger _logger;

    public CreateTransactionService(
        ICreateTransactionRepository transactionRepository,
        ICurrencyConversionService currencyConversionService,
        ITransactionServiceValidator transactionServiceValidator,
        ISeqLogger logger)
    {
        _createtransactionRepository = transactionRepository;
        _currencyConversionService = currencyConversionService;
        _transactionServiceValidator = transactionServiceValidator;
        _logger = logger;
    }

    public async Task<CreateTransactionResponse> ProcessWithdrawalTransactionAsync(CreateTransactionRequest request)
    {
        using var semaphore = new SemaphoreSlim(1, 1);

        await _transactionServiceValidator.ValidateCreateTransactionRequestAsync(request);
        await CheckAccountOwnershipAsync(request.FromAccountId, request.UserId);

        await semaphore.WaitAsync();

        var fromAccount = await _createtransactionRepository.GetAccountByIdAsync(request.FromAccountId);
        var toAccount = await _createtransactionRepository.GetAccountByIdAsync(request.ToAccountId);

        var transactionFee = CalculateTransactionFee(request.Amount, TransactionType.Atm);
        decimal convertedAmount = _currencyConversionService.Convert(request.Amount, request.Currency, request.ToCurrency);

        if (fromAccount.InitialAmount < request.Amount + transactionFee)
        {
            throw new InvalidOperationException("Insufficient funds for withdrawal.");
        }

        var transaction = new Transaction
        {
            FromAccountId = request.FromAccountId,
            FromAccountCurrency = request.Currency,
            ToAccountCurrency = request.ToCurrency,
            FromAmount = request.Amount + transactionFee,
            ToAmount = convertedAmount,
            Fee = transactionFee,
            TransactionType = (int)TransactionType.Atm,
        };

        await _createtransactionRepository.UpdateAccountBalancesAsync(transaction);

        semaphore.Release();

        return new CreateTransactionResponse
        {
            FromAccountId = transaction.FromAccountId,
            Amount = transaction.FromAmount,
            Currency = transaction.FromAccountCurrency,
            Fee = transaction.Fee,
            TransactionDate = transaction.TransactionDate
        };
    }

    public decimal CalculateTransactionFee(decimal amount, TransactionType transactionType)
    {
        decimal feePercentage = 0;
        decimal fixedFee = 0;

        if (transactionType == TransactionType.External)
        {
            feePercentage = 0.01M;
            fixedFee = 0.5M;
        }

        decimal totalFee = amount * feePercentage + fixedFee;
        return totalFee;
    }

    public async Task<bool> CheckAccountOwnershipAsync(int accountId, string userId)
    {
        var isCorrectAccount = await _createtransactionRepository.CheckAccountOwnershipAsync(accountId, userId);
        if (!isCorrectAccount)
        {
            throw new InvalidAccountException("User with Id: {UserId} trying to access someone else's BankAccount with Id: {AccountId}", userId, accountId);
        }

        return isCorrectAccount;
    }
}
