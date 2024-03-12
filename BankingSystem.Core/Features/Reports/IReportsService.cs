using BankingSystem.Core.Features.Reports.Dto_s;

public interface IReportsService
{
	Task<TransactionStatisticsDto> GetTransactionStatisticsAsync(DateTime startDate, DateTime endDate);
	Task<IEnumerable<DailyTransactionCountDto>> GetDailyTransactionCountsAsync(DateTime startDate, DateTime endDate);
	Task<TotalWithdrawnAmountDto> GetTotalWithdrawnAmountAsync(DateTime startDate, DateTime endDate);
	Task<TransactionStatisticsDto> GetAverageRevenuePerTransactionAsync(DateTime startDate, DateTime endDate);
	Task<UserStatisticsDto> GetUserStatisticsAsync();
}