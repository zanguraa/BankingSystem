using BankingSystem.Core.Features.Atm.ViewBalance.Requests;
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

}
