using BankingSystem.Core.Features.Reports.Requests;

namespace BankingSystem.Core.Features.Reports;
public interface IReportsService
{
    Task<TransactionStatisticsDto> GetAverageRevenuePerTransactionAsync(ReportsRequest request);
    Task<Dictionary<string, int>> GetDailyTransactionCountsAsync(ReportsRequest request);
    Task<TotalWithdrawnAmountDto> GetTotalWithdrawnAmountAsync(ReportsRequest request);
    Task<TransactionStatisticsDto> GetTransactionStatisticsAsync(ReportsRequest request);
    Task<UserStatisticsDto> GetUserStatisticsAsync();
}

public class ReportsService : IReportsService
{

    private readonly IReportsRepository _reportsRepository;

    public ReportsService(IReportsRepository reportsRepository)
    {
        _reportsRepository = reportsRepository;
    }

    public async Task<TransactionStatisticsDto> GetTransactionStatisticsAsync(ReportsRequest request)
    {
        var statisticsAggregate = await _reportsRepository.GetTransactionStatisticsAsync(request.StartDate, request.EndDate);

        return new TransactionStatisticsDto
        {
            TransactionsCount = statisticsAggregate.TransactionsCount,
            IncomeGEL = statisticsAggregate.IncomeGEL,
            IncomeUSD = statisticsAggregate.IncomeUSD,
            IncomeEUR = statisticsAggregate.IncomeEUR
        };
    }

    public async Task<Dictionary<string, int>> GetDailyTransactionCountsAsync(ReportsRequest request)
    {
        var transactionCountsFromRepo = await _reportsRepository.GetDailyTransactionCountsAsync(request.StartDate, request.EndDate);

        // Convert the list to a dictionary with the date as the key and transaction count as the value for faster lookups
        var transactionCountsDict = transactionCountsFromRepo
                                    .ToDictionary(tc => tc.Date.ToString("yyyy-MM-dd"), tc => tc.TransactionCount);

        var completeTransactionCounts = new Dictionary<string, int>();

        for (var date = request.StartDate; date <= request.EndDate; date = date.AddDays(1))
        {
            var dateString = date.ToString("yyyy-MM-dd");
            // Check if the dictionary contains the date as a string
            if (transactionCountsDict.TryGetValue(dateString, out var transactionCount))
            {
                completeTransactionCounts[dateString] = transactionCount;
            }
            else
            {
                completeTransactionCounts[dateString] = 0;
            }
        }

        return completeTransactionCounts;
    }

    public async Task<TotalWithdrawnAmountDto> GetTotalWithdrawnAmountAsync(ReportsRequest request)
    {
        return await _reportsRepository.GetTotalWithdrawnAmountAsync(request.StartDate, request.EndDate);
    }

    public async Task<TransactionStatisticsDto> GetAverageRevenuePerTransactionAsync(ReportsRequest request)
    {
        return await _reportsRepository.GetAverageRevenuePerTransactionAsync(request.StartDate, request.EndDate);
    }

    public async Task<UserStatisticsDto> GetUserStatisticsAsync()
    {
        return await _reportsRepository.GetUserStatisticsAsync();
    }

}

