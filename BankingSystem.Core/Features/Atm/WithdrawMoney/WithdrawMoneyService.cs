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

    public async Task<WithdrawResponse> WithdrawAsync(WithdrawRequest requestDto)
    {

        var card = await _cardAuthorizationRepository.GetCardByNumberAsync(requestDto.CardNumber);
        if (card == null)
        {
            return new WithdrawResponse { IsSuccessful = false, Message = "card not found." };
        }

        var accountInfo = await _viewBalanceRepository.GetBalanceInfoByCardNumberAsync(card.CardNumber);

       

        decimal amountToDeduct = requestDto.Amount;
        if (requestDto.Currency != accountInfo.Currency)
        {
            // Convert the withdrawal amount to the account's currency using the conversion service
            amountToDeduct = _currencyConversionService.Convert(
                requestDto.Amount,
                requestDto.Currency,
                 accountInfo.Currency
            );
        }

        // Calculate the commission on the amount to be deducted
        var commission = amountToDeduct * 0.02m;
        var totalDeduction = amountToDeduct + commission;

        // Check if the total amount to deduct (including commission) exceeds the account balance
        if (totalDeduction > accountInfo.InitialAmount)
        {
            return new WithdrawResponse { IsSuccessful = false, Message = "Insufficient funds.", RemainingBalance = accountInfo.InitialAmount };
        }

        var report24HoursRequest = new WithdrawalCheck() { BankAccountId = card.AccountId, WithdrawalDate = DateTime.Now.AddDays(-1) };

        // Check the total withdrawal amount in the last 24 hours to ensure it's within the daily limit
        var totalWithdrawnAmountInGel = await _withdrawMoneyRepository.GetWithdrawalsOf24hoursByCardId(report24HoursRequest);
        if (totalWithdrawnAmountInGel.Sum + totalDeduction > _dailyWithdrawalLimitInGel)
        {
            return new WithdrawResponse { IsSuccessful = false, Message = "Daily withdrawal limit exceeded.", RemainingBalance = accountInfo.InitialAmount };
        }


        WithdrawRequest withdrawRequest = new()
        {
            AccountId = card.AccountId,
            Currency = requestDto.Currency,
            Amount = totalDeduction,
        };


        var result = await _withdrawMoneyRepository.WithdrawAsync(withdrawRequest);

        return new WithdrawResponse
        {
            IsSuccessful = true,
            Message = $"Withdrawal of {requestDto.Amount} {requestDto.Currency} was successful.",
            RemainingBalance = accountInfo.InitialAmount - totalDeduction,
            Commision = commission
        };
    }
}
