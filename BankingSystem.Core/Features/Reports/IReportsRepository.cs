using BankingSystem.Core.Features.Reports.Requests;

namespace BankingSystem.Core.Features.Reports
{
    public interface IReportsRepository
    {
        Task<TransactionStatisticsDto> GetAverageRevenuePerTransactionAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<DailyTransactionCountDto>> GetDailyTransactionCountsAsync(DateTime startDate, DateTime endDate);
        Task<TotalWithdrawnAmountDto> GetTotalWithdrawnAmountAsync(DateTime startDate, DateTime endDate);
        Task<TransactionStatisticsDto> GetTransactionStatisticsAsync(DateTime startDate, DateTime endDate);
        Task<UserStatisticsDto> GetUserStatisticsAsync();
    }
}