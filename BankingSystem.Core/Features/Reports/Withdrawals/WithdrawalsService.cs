using BankingSystem.Core.Features.Reports.Shared.Requests;

namespace BankingSystem.Core.Features.Reports.Withdrawals;

public interface IWithdrawalsService
{
    Task<TotalWithdrawnAmountDto> GetTotalWithdrawnAmountAsync(ReportsRequest request);
}

public class WithdrawalsService : IWithdrawalsService
{
    private readonly IWithdrawalsRepository _withdrawalRepository;

    public WithdrawalsService(IWithdrawalsRepository withdrawalsRepository)
    {
        _withdrawalRepository = withdrawalsRepository;

    }
    public async Task<TotalWithdrawnAmountDto> GetTotalWithdrawnAmountAsync(ReportsRequest request)
    {
        return await _withdrawalRepository.GetTotalWithdrawnAmountAsync(request.StartDate, request.EndDate);
    }
}
