using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Features.Atm.WithdrawMoney.Models.Requests;
using BankingSystem.Core.Features.Atm.WithdrawMoney.Models.Response;
using BankingSystem.Core.Shared.Models;
using BankingSystem.Core.Shared.Services.Currency;

namespace BankingSystem.Core.Features.Atm.WithdrawMoney;

public interface IWithdrawMoneyService
{
    Task<WithdrawResponse> WithdrawAsync(WithdrawAmountCurrencyRequest requestDto, string? cardNumber);
}

public class WithdrawMoneyService : IWithdrawMoneyService
{
    private readonly IWithdrawMoneyRepository _withdrawMoneyRepository;
    private readonly ICurrencyConversionService _currencyConversionService;
    private readonly int _dailyWithdrawalLimitInGel = 10000;
    private readonly ISeqLogger _seqLogger;

    public WithdrawMoneyService(
        IWithdrawMoneyRepository withdrawMoneyRepository,
        ICurrencyConversionService currencyConversionService,
        ISeqLogger seqLogger
        )
    {
        _withdrawMoneyRepository = withdrawMoneyRepository;
        _currencyConversionService = currencyConversionService;
        _seqLogger = seqLogger;
    }

    public async Task<WithdrawResponse> WithdrawAsync(WithdrawAmountCurrencyRequest requestDto, string? cardNumber)
    {
        requestDto.CardNumber = cardNumber;

        ValidateWithdrawRequest(requestDto);

        var card = await _withdrawMoneyRepository.GetCardByNumberAsync(cardNumber) ?? throw new InvalidCardException("Card not found: {card}", requestDto.CardNumber);
        var accountInfo = await _withdrawMoneyRepository.GetBalanceInfoByCardNumberAsync(card.CardNumber) ?? throw new InvalidBalanceException("Balance info not found for card: {card}", card.CardNumber);

        decimal amountToDeduct = requestDto.Amount;
        if (requestDto.Currency != accountInfo.Currency)
        {
            amountToDeduct = _currencyConversionService.Convert(requestDto.Amount, requestDto.Currency, accountInfo.Currency);
        }

        decimal commission = amountToDeduct * 0.02m;
        decimal totalDeduction = amountToDeduct + commission;

        if (totalDeduction > accountInfo.InitialAmount)
            return new() { IsSuccessful = false, Message = "Insufficient funds.", RemainingBalance = accountInfo.InitialAmount };


        var report24HoursRequest = new WithdrawalCheck { BankAccountId = card.AccountId, WithdrawalDate = DateTime.Now.AddDays(-1) };
        var totalWithdrawnAmountInGel = await _withdrawMoneyRepository.GetWithdrawalsOf24hoursByCardId(report24HoursRequest);

        // საკითხავია სწორია თუ არა ვანოსთან
        if (totalWithdrawnAmountInGel?.Sum + totalDeduction > _dailyWithdrawalLimitInGel)
        {
            throw new InvalidAtmAmountException("Daily limit Daily withdrawal limit exceeded:{Amount} in {Currency}, for Card: {Card}", requestDto.Amount, requestDto.Currency, requestDto.CardNumber);
        }

        var transactionType = TransactionType.Atm;

        var transaction = new Transaction
        {
            FromAccountId = card.AccountId,
            FromAccountCurrency = accountInfo.Currency,
            ToAccountId = card.AccountId,
            FromAmount = amountToDeduct,
            ToAmount = requestDto.Amount,
            Fee = commission,
            TransactionType = (int)transactionType,
            TransactionDate = DateTime.UtcNow
        };

        bool withdrawalSuccess = await _withdrawMoneyRepository.ProcessAtmTransaction(transaction);

        var withdrawalResult = new WithdrawResponse
        {
            IsSuccessful = withdrawalSuccess,
            Message = withdrawalSuccess ? "Withdrawal successful." : "Withdrawal failed.",
            RemainingBalance = withdrawalSuccess ? accountInfo.InitialAmount - totalDeduction : accountInfo.InitialAmount,
            Commission = commission,
            RequestedAmount = requestDto.Amount,
            RequestedCurrency = requestDto.Currency,
            DeductedAmount = withdrawalSuccess ? amountToDeduct : 0,
            AccountCurrency = accountInfo.Currency,
            WithdrawalDate = DateTime.UtcNow
        };

        _seqLogger.LogInfo("Withdrawal Successfull. account {accountId}, amount {currnecy} {amount}, commision {commision}", card.AccountId, withdrawalResult.RequestedCurrency, requestDto.Amount, withdrawalResult.Commission);

        return withdrawalResult;
    }

    private void ValidateWithdrawRequest(WithdrawAmountCurrencyRequest requestDto)
    {
		if (requestDto == null)
		{
			throw new ArgumentNullException(nameof(requestDto), "The request cannot be null.");
		}

		if (requestDto.Amount < 5 || requestDto.Amount % 5 != 0)
        {
            throw new InvalidAtmAmountException("Invalid withdrawal amount. Amount must be in multiples of 5");
        }
        if (requestDto.Amount > _dailyWithdrawalLimitInGel)
        {
            throw new InvalidAtmAmountException("Amount exceeds withdrawal limit");
        }
        if (!Enum.TryParse<Currency>(requestDto.Currency, out var currency) || !Enum.IsDefined(typeof(Currency), currency))
        {
            throw new UnsupportedCurrencyException("Unsupported currency");
        }

    }
}
