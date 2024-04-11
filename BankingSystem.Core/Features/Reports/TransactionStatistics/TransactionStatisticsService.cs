using BankingSystem.Core.Features.Reports.Shared.Requests;

namespace BankingSystem.Core.Features.Reports.TransactionStatistics;

public interface ITransactionStatisticsService
{
    Task<TransactionStatisticsResponse> GetAverageRevenuePerTransactionAsync(ReportsRequest request);
    Task<Dictionary<string, int>> GetDailyTransactionCountsAsync(ReportsRequest request);
    Task<TransactionStatisticsResponse> GetTransactionStatisticsAsync(ReportsRequest request);
}

public class TransactionStatisticsService : ITransactionStatisticsService
{
    private readonly ITransactionStatisticsRepository _transactionStatisticsRepository;
    public TransactionStatisticsService(ITransactionStatisticsRepository transactionStatisticsRepository)
    {
        _transactionStatisticsRepository = transactionStatisticsRepository;
    }

    public async Task<TransactionStatisticsResponse> GetTransactionStatisticsAsync(ReportsRequest request)
    {
        var statisticsAggregate = await _transactionStatisticsRepository.GetTransactionStatisticsAsync(request.StartDate, request.EndDate);

        return new TransactionStatisticsResponse
        {
            TransactionsCount = statisticsAggregate.TransactionsCount,
            TransactionsInGEL = statisticsAggregate.TransactionsInGEL,
            TransactionsInUSD = statisticsAggregate.TransactionsInUSD,
            TransactionsInEUR = statisticsAggregate.TransactionsInEUR
        };
    }

    public async Task<Dictionary<string, int>> GetDailyTransactionCountsAsync(ReportsRequest request)
    {
        var transactionCountsFromRepo = await _transactionStatisticsRepository.GetDailyTransactionCountsAsync(request.StartDate, request.EndDate);

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

    public async Task<TransactionStatisticsResponse> GetAverageRevenuePerTransactionAsync(ReportsRequest request)
    {
        return await _transactionStatisticsRepository.GetAverageRevenuePerTransactionAsync(request.StartDate, request.EndDate);
    }

}
