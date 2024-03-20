using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankingSystem.Core.Features.Reports;
using BankingSystem.Core.Features.Reports.Requests;

public class ReportsService : IReportsService
{
	private readonly IReportsRepository _reportsRepository;

	public ReportsService(IReportsRepository reportsRepository)
	{
		_reportsRepository = reportsRepository;
	}

    public async Task<TransactionStatisticsDto> GetTransactionStatisticsAsync(DateTime startDate, DateTime endDate)
    {
        var statisticsAggregate = await _reportsRepository.GetTransactionStatisticsAsync(startDate, endDate);

        return new TransactionStatisticsDto
        {
            TransactionsCount = statisticsAggregate.TransactionsCount,
            IncomeGEL = statisticsAggregate.IncomeGEL,
            IncomeUSD = statisticsAggregate.IncomeUSD,
            IncomeEUR = statisticsAggregate.IncomeEUR
        };
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

