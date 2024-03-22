using BankingSystem.Core.Features.Atm.ViewBalance;


public interface IViewBalanceRepository
{
	Task<BalanceInfo?> GetBalanceUserIdAsync(string userId);
	Task<BalanceInfo?> GetBalanceInfoByUserIdAsync(string userId);
	Task<BalanceInfo?> GetBalanceInfoByCardNumberAsync(string cardNumber);
}