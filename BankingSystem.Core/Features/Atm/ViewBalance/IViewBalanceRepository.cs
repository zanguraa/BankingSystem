using BankingSystem.Core.Features.Atm.ViewBalance;
using System.Threading.Tasks;


public interface IViewBalanceRepository
{
	Task<BalanceInfo?> GetBalanceUserIdAsync(string userId);
	Task<BalanceInfo?> GetBalanceInfoByUserIdAsync(string userId);
}