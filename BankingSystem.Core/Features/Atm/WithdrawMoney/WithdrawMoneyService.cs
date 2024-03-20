using BankingSystem.Core.Features.Atm.WithdrawMoney.Requests;
using BankingSystem.Core.Features.Atm.WithdrawMoney;
using BankingSystem.Core.Features.Transactions.Currency;
using BankingSystem.Core.Features.Atm.CardAuthorization;

public class WithdrawMoneyService : IWithdrawMoneyService
{
    private readonly IWithdrawMoneyRepository _withdrawMoneyRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ICurrencyConversionService _currencyConversionService;
    private readonly int _dailyWithdrawalLimitInGel = 10000;
    private readonly ICardAuthorizationRepository _cardAuthorizationRepository;
    public readonly IViewBalanceRepository _viewBalanceRepository;


    public WithdrawMoneyService(
        IWithdrawMoneyRepository withdrawMoneyRepository,
        IBankAccountRepository bankAccountRepository,
        ICurrencyConversionService currencyConversionService,
        ICardAuthorizationRepository cardAuthorizationRepository,
        IViewBalanceRepository viewBalanceRepository)
    {
        _withdrawMoneyRepository = withdrawMoneyRepository;
        _bankAccountRepository = bankAccountRepository;
        _currencyConversionService = currencyConversionService;
        _cardAuthorizationRepository = cardAuthorizationRepository;
        _viewBalanceRepository = viewBalanceRepository;

    }

    public async Task<WithdrawResponse> WithdrawAsync(WithdrawRequestWithCardNumber requestDto)
    {
        var card = await _cardAuthorizationRepository.GetCardByNumberAsync(requestDto.CardNumber);
        if (card == null)
        {
            return new WithdrawResponse { IsSuccessful = false, Message = "Card not found." };
        }

        var accountInfo = await _viewBalanceRepository.GetBalanceInfoByCardNumberAsync(card.CardNumber);
        if (accountInfo == null)
        {
            return new WithdrawResponse { IsSuccessful = false, Message = "Account information not found." };
        }

        if (requestDto.Amount % 5 != 0 || requestDto.Amount < 5 || requestDto.Amount > _dailyWithdrawalLimitInGel)
        {
            return new WithdrawResponse { IsSuccessful = false, Message = "Invalid withdrawal amount. Amount must be in multiples of 5 and within the daily limit." };
        }

        decimal amountToDeduct = requestDto.Amount;
        if (requestDto.Currency != accountInfo.Currency)
        {
            amountToDeduct = _currencyConversionService.Convert(requestDto.Amount, requestDto.Currency, accountInfo.Currency);
        }

        decimal commission = amountToDeduct * 0.02m;
        decimal totalDeduction = amountToDeduct + commission;

        if (totalDeduction > accountInfo.InitialAmount)
        {
            return new WithdrawResponse { IsSuccessful = false, Message = "Insufficient funds.", RemainingBalance = accountInfo.InitialAmount };
        }

        var report24HoursRequest = new WithdrawalCheck { BankAccountId = card.AccountId, WithdrawalDate = DateTime.Now.AddDays(-1) };
        var totalWithdrawnAmountInGel = await _withdrawMoneyRepository.GetWithdrawalsOf24hoursByCardId(report24HoursRequest);

        if (totalWithdrawnAmountInGel.Sum + totalDeduction > _dailyWithdrawalLimitInGel)
        {
            return new WithdrawResponse { IsSuccessful = false, Message = "Daily withdrawal limit exceeded.", RemainingBalance = accountInfo.InitialAmount };
        }

        var withdrawRequest = new WithdrawRequest
        {
            AccountId = card.AccountId, 
            Amount = amountToDeduct, 
            Currency = accountInfo.Currency, 
            RequestedAmount = requestDto.Amount, 
            RequestedCurrency = requestDto.Currency, 
        };

        bool withdrawalSuccess = await _withdrawMoneyRepository.WithdrawAsync(withdrawRequest);

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
