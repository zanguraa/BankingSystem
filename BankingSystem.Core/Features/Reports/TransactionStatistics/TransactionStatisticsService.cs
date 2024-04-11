using BankingSystem.Core.Features.Reports.Shared.Requests;
using BankingSystem.Core.Shared.Exceptions;

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
        ValidateReportsRequest(request);

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
        ValidateReportsRequest(request);

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
        ValidateReportsRequest(request);

        return await _transactionStatisticsRepository.GetAverageRevenuePerTransactionAsync(request.StartDate, request.EndDate);
    }

    private void ValidateReportsRequest(ReportsRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "ReportsRequest cannot be null.");
        }

        if (request.StartDate == DateTime.MinValue)
        {
            throw new InvalidReportsException("StartDate is required.", nameof(request.StartDate));
        }

        if (request.EndDate == DateTime.MinValue)
        {
            request.EndDate = DateTime.Now;
        }

        if (request.StartDate > request.EndDate)
        {
            throw new InvalidReportsException("StartDate cannot be greater than EndDate.", nameof(request.StartDate));
        }

        if (request.StartDate > DateTime.Now || request.EndDate > DateTime.Now)
        {
            throw new InvalidReportsException("StartDate and EndDate cannot be in the future.", nameof(request.StartDate));
        }
    }
}


