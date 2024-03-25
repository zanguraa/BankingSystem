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

    public async Task<Dictionary<string, int>> GetDailyTransactionCountsAsync(DateTime startDate, DateTime endDate)
    {
        var transactionCountsFromRepo = await _reportsRepository.GetDailyTransactionCountsAsync(startDate, endDate);

        // Convert the list to a dictionary with the date as the key and transaction count as the value for faster lookups
        var transactionCountsDict = transactionCountsFromRepo
                                    .ToDictionary(tc => tc.Date.ToString("yyyy-MM-dd"), tc => tc.TransactionCount);

        var completeTransactionCounts = new Dictionary<string, int>();

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
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

