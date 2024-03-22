using BankingSystem.Core.Features.Atm.ViewBalance.Requests;

namespace BankingSystem.Core.Features.Atm.ViewBalance;

public interface IViewBalanceService
{
    Task<BalanceResponse> GetBalanceByUserIdAsync(string userId);
    Task<BalanceResponse> GetBalanceByCardNumberAsync(string cardNumber);
}