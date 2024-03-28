using BankingSystem.Core.Features.Transactions.CreateTransactions.Models.Requests;
using BankingSystem.Core.Features.Transactions.CreateTransactions.Models.Response;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Models;
using BankingSystem.Core.Shared.Services.Currency;

namespace BankingSystem.Core.Features.Transactions.CreateTransactions;

public interface ICreateTransactionService
{
    Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId);
    Task<CreateTransactionResponse> ProcessInternalTransactionAsync(CreateTransactionRequest request);
    Task<CreateTransactionResponse> ProcessExternalTransactionAsync(CreateTransactionRequest request);
    Task<CreateTransactionResponse> ProcessWithdrawalTransactionAsync(CreateTransactionRequest request);
    Task<bool> CheckAccountOwnershipAsync(int accountId, string userId);
}

public class CreateTransactionService : ICreateTransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICurrencyConversionService _currencyConversionService;
    private readonly ITransactionServiceValidator _transactionServiceValidator;

    public CreateTransactionService(
        ITransactionRepository transactionRepository,
        ICurrencyConversionService currencyConversionService,
        ITransactionServiceValidator transactionServiceValidator)
    {
        _transactionRepository = transactionRepository;
        _currencyConversionService = currencyConversionService;
        _transactionServiceValidator = transactionServiceValidator;
    }

    public async Task<CreateTransactionResponse> ProcessInternalTransactionAsync(CreateTransactionRequest request)
    {
        using var semaphore = new SemaphoreSlim(1, 1);

        await _transactionServiceValidator.ValidateCreateTransactionRequest(request);
        await CheckAccountOwnershipAsync(request.FromAccountId, request.UserId);

        await semaphore.WaitAsync();

        var fromAccount = await _transactionRepository.GetAccountByIdAsync(request.FromAccountId);
        var toAccount = await _transactionRepository.GetAccountByIdAsync(request.ToAccountId);

        decimal convertedAmount = _currencyConversionService.Convert(request.Amount, request.Currency, request.ToCurrency);


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
            ToAmount = convertedAmount,
            Fee = 0,
            TransactionType = (int)TransactionType.Internal,
            TransactionDate = DateTime.UtcNow
        };

        await _transactionRepository.ProcessBankTransaction(transaction);

        semaphore.Release();

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

    public async Task<CreateTransactionResponse> ProcessExternalTransactionAsync(CreateTransactionRequest request)
    {
        using var semaphore = new SemaphoreSlim(1, 1);

        await _transactionServiceValidator.ValidateCreateTransactionRequest(request);
        await CheckAccountOwnershipAsync(request.FromAccountId, request.UserId);

        await semaphore.WaitAsync();

        var transactionFee = CalculateTransactionFee(request.Amount, TransactionType.External);
        var convertedAmount = _currencyConversionService.Convert(request.Amount, request.Currency, request.ToCurrency);

        var fromAccount = await _transactionRepository.GetAccountByIdAsync(request.FromAccountId);
        var toAccount = await _transactionRepository.GetAccountByIdAsync(request.ToAccountId);
        if (fromAccount.InitialAmount < request.Amount + transactionFee)
        {
            throw new InvalidOperationException("Insufficient funds for external transaction.");
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
            TransactionType = (int)TransactionType.External,
            TransactionDate = DateTime.Now,
        };

        await _transactionRepository.ProcessBankTransaction(transaction);

        semaphore.Release();

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

    public async Task<CreateTransactionResponse> ProcessWithdrawalTransactionAsync(CreateTransactionRequest request)
    {
        using var semaphore = new SemaphoreSlim(1, 1);

        await _transactionServiceValidator.ValidateCreateTransactionRequest(request);
        await CheckAccountOwnershipAsync(request.FromAccountId, request.UserId);

        await semaphore.WaitAsync();

        var fromAccount = await _transactionRepository.GetAccountByIdAsync(request.FromAccountId);
        var toAccount = await _transactionRepository.GetAccountByIdAsync(request.ToAccountId);

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

        await _transactionRepository.UpdateAccountBalancesAsync(transaction);

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


    public async Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId)
    {
        return await _transactionRepository.GetTransactionsByAccountIdAsync(accountId);
    }

    public async Task<bool> CheckAccountOwnershipAsync(int accountId, string userId)
    {
        var isCorrectAccount = await _transactionRepository.CheckAccountOwnershipAsync(accountId, userId);
        if (!isCorrectAccount)
        {
            throw new Exception("You do not have permission to access this account.");
        }

        return isCorrectAccount;
    }

    private decimal CalculateTransactionFee(decimal amount, TransactionType transactionType)
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

}
