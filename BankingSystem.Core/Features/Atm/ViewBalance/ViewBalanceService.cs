using BankingSystem.Core.Features.Atm.ViewBalance.Dto_s;
using BankingSystem.Core.Features.Atm.ViewBalance;

public class ViewBalanceService : IViewBalanceService
{
	private readonly IViewBalanceRepository _viewBalanceRepository;

	public ViewBalanceService(IViewBalanceRepository viewBalanceRepository)
	{
		_viewBalanceRepository = viewBalanceRepository;
	}

	public async Task<BalanceResponseDto> GetBalanceByUserIdAsync(string userId)
	{
		var balanceInfo = await _viewBalanceRepository.GetBalanceInfoByUserIdAsync(userId);

		if (balanceInfo == null)
		{
			throw new KeyNotFoundException($"No balance information found for user ID: {userId}.");
		}

		return new BalanceResponseDto
		{
			UserId = balanceInfo.UserId,
			InitialAmount = balanceInfo.InitialAmount,
			Currency = balanceInfo.Currency
		};
	}
}
