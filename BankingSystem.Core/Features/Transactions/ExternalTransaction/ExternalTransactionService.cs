using BankingSystem.Core.Features.Transactions.Shared;
using BankingSystem.Core.Features.Transactions.Shared.Models.Requests;
using BankingSystem.Core.Features.Transactions.Shared.Models.Response;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Models;
using BankingSystem.Core.Shared.Services.Currency;

namespace BankingSystem.Core.Features.Transactions.CreateTransactions;

public interface IExternalTransactionService
{
    Task<CreateTransactionResponse> ProcessExternalTransactionAsync(CreateTransactionRequest request);
}

public class ExternalTransactionService : IExternalTransactionService
{
    private readonly ICreateTransactionRepository _createtransactionRepository;
    private readonly ICurrencyConversionService _currencyConversionService;
    private readonly ITransactionServiceValidator _transactionServiceValidator;
    private readonly ICreateTransactionService _createTransactionService;
    private readonly ISeqLogger _logger;

    public ExternalTransactionService(
        ICreateTransactionRepository transactionRepository,
        ICurrencyConversionService currencyConversionService,
        ITransactionServiceValidator transactionServiceValidator,
        ICreateTransactionService createTransactionService,
        ISeqLogger logger)
    {
        _createtransactionRepository = transactionRepository;
        _currencyConversionService = currencyConversionService;
        _transactionServiceValidator = transactionServiceValidator;
        _createTransactionService = createTransactionService;
        _logger = logger;
    }

    public async Task<CreateTransactionResponse> ProcessExternalTransactionAsync(CreateTransactionRequest request)
    {
        using var semaphore = new SemaphoreSlim(1, 1);

        await _transactionServiceValidator.ValidateCreateTransactionRequest(request);
        await _createTransactionService.CheckAccountOwnershipAsync(request.FromAccountId, request.UserId);

        await semaphore.WaitAsync();

        var transactionFee = CalculateTransactionFee(request.Amount, TransactionType.External);
        var convertedAmount = _currencyConversionService.Convert(request.Amount, request.Currency, request.ToCurrency);

        var fromAccount = await _createtransactionRepository.GetAccountByIdAsync(request.FromAccountId);
        var toAccount = await _createtransactionRepository.GetAccountByIdAsync(request.ToAccountId);
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

        await _createtransactionRepository.ProcessBankTransaction(transaction);

        semaphore.Release();

        _logger.LogInfo("External Transaction! fromAccount: {fromAccount}, was sent money: {amount}-{currency} toAccountId {toAccountId}", request.FromAccountId, request.Amount, request.ToCurrency, request.ToAccountId);


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
