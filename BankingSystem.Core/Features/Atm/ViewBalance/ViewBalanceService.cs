using BankingSystem.Core.Features.Atm.ViewBalance.Requests;
using BankingSystem.Core.Features.Atm.ViewBalance;

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

		if (balanceInfo == null)
		{
			throw new KeyNotFoundException($"No balance information found for user ID: {userId}.");
		}

		return new BalanceResponse
		{
			UserId = balanceInfo.UserId,
			InitialAmount = balanceInfo.InitialAmount,
			Currency = balanceInfo.Currency
		};
	}
}
