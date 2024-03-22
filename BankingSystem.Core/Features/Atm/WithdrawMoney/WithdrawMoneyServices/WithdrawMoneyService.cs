﻿using BankingSystem.Core.Features.Atm.WithdrawMoney.Requests;
using BankingSystem.Core.Features.Transactions.Currency;
using BankingSystem.Core.Features.Atm.CardAuthorization;
using BankingSystem.Core.Features.Transactions.TransactionServices;
using BankingSystem.Core.Features.Transactions;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Core.Features.Atm.WithdrawMoney.WithdrawMoneyServices;
using BankingSystem.Core.Features.Atm.WithdrawMoney.WithdrawMoneyRepository;
using BankingSystem.Core.Features.Transactions.TransactionsRepositories;



public class WithdrawMoneyService : IWithdrawMoneyService
{
    private readonly IWithdrawMoneyRepository _withdrawMoneyRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ICurrencyConversionService _currencyConversionService;
    private readonly int _dailyWithdrawalLimitInGel = 10000;
    private readonly ICardAuthorizationRepository _cardAuthorizationRepository;
    public readonly IViewBalanceRepository _viewBalanceRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IWithdrawMoneyServiceValidator _withdrawMoneyServiceValidator;

    public WithdrawMoneyService(
        IWithdrawMoneyRepository withdrawMoneyRepository,
        IBankAccountRepository bankAccountRepository,
        ICurrencyConversionService currencyConversionService,
        ICardAuthorizationRepository cardAuthorizationRepository,
        IViewBalanceRepository viewBalanceRepository,
        ITransactionRepository transactionRepository,
        IWithdrawMoneyServiceValidator withdrawMoneyServiceValidator
        )
    {
        _withdrawMoneyRepository = withdrawMoneyRepository;
        _bankAccountRepository = bankAccountRepository;
        _currencyConversionService = currencyConversionService;
        _cardAuthorizationRepository = cardAuthorizationRepository;
        _viewBalanceRepository = viewBalanceRepository;
        _transactionRepository = transactionRepository;
        _withdrawMoneyServiceValidator = withdrawMoneyServiceValidator;
    }

    public async Task<WithdrawResponse> WithdrawAsync(WithdrawRequestWithCardNumber requestDto)
    {
        _withdrawMoneyServiceValidator.ValidateWithdrawRequest(requestDto);

        var card = await _cardAuthorizationRepository.GetCardByNumberAsync(requestDto.CardNumber);
        if (card == null)
            return new() { IsSuccessful = false, Message = "Card not found." };


        var accountInfo = await _viewBalanceRepository.GetBalanceInfoByCardNumberAsync(card.CardNumber);
        if (accountInfo == null)
            return new() { IsSuccessful = false, Message = "Account information not found." };


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

        if (totalWithdrawnAmountInGel.Sum + totalDeduction > _dailyWithdrawalLimitInGel)
            return new() { IsSuccessful = false, Message = "Daily withdrawal limit exceeded.", RemainingBalance = accountInfo.InitialAmount };

        var transactionType = TransactionType.Atm;

        var transaction = new Transaction
        {
            FromAccountId = card.AccountId,
            FromAccountCurrency = accountInfo.Currency,
            ToAccountCurrency = requestDto.Currency,
            ToAccountId = card.AccountId,
            FromAmount = amountToDeduct,
            ToAmount = requestDto.Amount,
            Fee = commission,
            TransactionType = (int)transactionType,
            TransactionDate = DateTime.UtcNow
        };

        bool withdrawalSuccess = await _transactionRepository.UpdateAccountBalancesAsync(transaction, true);


        var logEntry = new TransactionLog
        {
            RequestedAmount = requestDto.Amount,
            RequestedCurrency = requestDto.Currency,
            DeductedAmount = withdrawalSuccess ? amountToDeduct : 0,
            AccountCurrency = accountInfo.Currency,
            BankAccountId = card.AccountId,
            WithdrawalDate = DateTime.UtcNow
        };

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

        return withdrawalResult;
    }
}