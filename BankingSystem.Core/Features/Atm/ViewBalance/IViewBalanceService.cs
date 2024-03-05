using BankingSystem.Core.Features.Atm.ViewBalance.Dto_s;

namespace BankingSystem.Core.Features.Atm.ViewBalance
{
	public interface IViewBalanceService
	{
		Task<BalanceResponseDto> GetBalanceByUserIdAsync(string userId);

	}
}