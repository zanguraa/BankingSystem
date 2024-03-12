using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankingSystem.Core.Features.Reports;
using BankingSystem.Core.Features.Reports.Dto_s;

public class ReportsService : IReportsService
{
	private readonly IReportsRepository _reportsRepository;

	public ReportsService(IReportsRepository reportsRepository)
	{
		_reportsRepository = reportsRepository;
	}

	public async Task<TransactionStatisticsDto> GetTransactionStatisticsAsync(DateTime startDate, DateTime endDate)
	{
		return await _reportsRepository.GetTransactionStatisticsAsync(startDate, endDate);
	}

	public async Task<IEnumerable<DailyTransactionCountDto>> GetDailyTransactionCountsAsync(DateTime startDate, DateTime endDate)
	{
		return await _reportsRepository.GetDailyTransactionCountsAsync(startDate, endDate);
	}

	public async Task<TotalWithdrawnAmountDto> GetTotalWithdrawnAmountAsync(DateTime startDate, DateTime endDate)
	{
		return await _reportsRepository.GetTotalWithdrawnAmountAsync(startDate, endDate);
	}

	public async Task<TransactionStatisticsDto> GetAverageRevenuePerTransactionAsync(DateTime startDate, DateTime endDate)
	{
		return await _reportsRepository.GetAverageRevenuePerTransactionAsync(startDate, endDate);
	}

	public async Task<UserStatisticsDto> GetUserStatisticsAsync()
	{
		return await _reportsRepository.GetUserStatisticsAsync();
	}
}

