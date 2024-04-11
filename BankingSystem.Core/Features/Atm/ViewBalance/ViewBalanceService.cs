using BankingSystem.Core.Features.Atm.ViewBalance.Models.Response;
using BankingSystem.Core.Shared.Exceptions;

namespace BankingSystem.Core.Features.Atm.ViewBalance;

public interface IViewBalanceService
{
    Task<BalanceResponse> GetBalanceByCardNumberAsync(string cardNumber);
    Task<BalanceResponse> GetBalanceByUserIdAsync(string userId);
}

public class ViewBalanceService : IViewBalanceService
{
    private readonly IViewBalanceRepository _viewBalanceRepository;

    public ViewBalanceService(IViewBalanceRepository viewBalanceRepository)
    {
        _viewBalanceRepository = viewBalanceRepository;
    }

    public async Task<BalanceResponse> GetBalanceByUserIdAsync(string userId)
    {
        var balanceInfo = await _viewBalanceRepository.GetBalanceInfoByUserIdAsync(userId);

        return balanceInfo == null
            ? throw new KeyNotFoundException($"No balance information found for user ID: {userId}.")
            : new BalanceResponse
            {
                UserId = balanceInfo.UserId,
                InitialAmount = balanceInfo.InitialAmount,
                Currency = balanceInfo.Currency
            };
    }

    public async Task<BalanceResponse> GetBalanceByCardNumberAsync(string cardNumber)
    {
        await GetbalanceByCardNumberValidatorAsync(cardNumber);

        var balanceInfo = await _viewBalanceRepository.GetBalanceInfoByCardNumberAsync(cardNumber);

        return balanceInfo == null
            ? throw new InvalidCardException($"No balance information found for card number: {cardNumber}.")
            : new BalanceResponse
            {
                UserId = balanceInfo.UserId,
                InitialAmount = balanceInfo.InitialAmount,
                Currency = balanceInfo.Currency
            };
    }

    private async Task GetbalanceByCardNumberValidatorAsync(string cardNumber)
    {
        if (string.IsNullOrEmpty(cardNumber))
            throw new InvalidCardException("Card Number is not Found.", cardNumber);

        var balanceInfo = await _viewBalanceRepository.GetBalanceInfoByCardNumberAsync(cardNumber);
        if (balanceInfo == null)
            throw new InvalidCardException("Card Number is not Found.", cardNumber);
    }

}
