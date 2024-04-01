
using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Reports.Shared.Requests;

namespace BankingSystem.Core.Features.Reports.TransactionStatistics
{
    public interface ITransactionStatisticsRepository
    {
        Task<TransactionStatisticsResponse> GetAverageRevenuePerTransactionAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<DailyTransactionCountDto>> GetDailyTransactionCountsAsync(DateTime startDate, DateTime endDate);
        Task<TransactionStatisticsResponse> GetTransactionStatisticsAsync(DateTime startDate, DateTime endDate);
    }

    public class TransactionStatisticsRepository : ITransactionStatisticsRepository
    {
        private readonly IDataManager _dataManager;
        public TransactionStatisticsRepository(IDataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public async Task<TransactionStatisticsResponse> GetTransactionStatisticsAsync(DateTime startDate, DateTime endDate)
        {

            const string transactionQuery = @"
                SELECT 
                    COUNT(TransactionId) AS NumberOfTransactions,
                    SUM(CASE WHEN FromAccountCurrency = 'GEL' THEN FromAmount ELSE 0 END) AS IncomeLastMonthInGEL,
                    SUM(CASE WHEN FromAccountCurrency = 'USD' THEN FromAmount ELSE 0 END) AS IncomeLastMonthInUSD,
                    SUM(CASE WHEN FromAccountCurrency = 'EUR' THEN FromAmount ELSE 0 END) AS IncomeLastMonthInEUR
                FROM [BankingSystem_db].[dbo].[Transactions]
                WHERE TransactionDate BETWEEN @startDate AND @endDate 
                ";


            var statisticsList = await _dataManager.Query<TransactionStatisticsAggregate, dynamic>(
                transactionQuery,
                new { startDate, endDate });

            var statistics = statisticsList.FirstOrDefault();

            if (statistics == null)
                return new TransactionStatisticsResponse();

            return new TransactionStatisticsResponse
            {
                TransactionsCount = statistics.NumberOfTransactions,
                TransactionsInGEL = statistics.IncomeLastMonthInGEL,
                TransactionsInUSD = statistics.IncomeLastMonthInUSD,
                TransactionsInEUR = statistics.IncomeLastMonthInEUR
            };
        }

        public async Task<IEnumerable<DailyTransactionCountDto>> GetDailyTransactionCountsAsync(DateTime startDate, DateTime endDate)
        {
            endDate = endDate.AddDays(1).AddSeconds(-1);

            const string dailyCountQuery = @"
                SELECT 
                    CAST([TransactionDate] AS DATE) AS Date,
                    COUNT(*) AS TransactionCount 
                FROM [BankingSystem_db].[dbo].[Transactions]
                WHERE [TransactionDate] >= @startDate AND [TransactionDate] <= @endDate
                GROUP BY CAST([TransactionDate] AS DATE) 
                ORDER BY Date;
                ";

            return await _dataManager.Query<DailyTransactionCountDto, dynamic>(
                dailyCountQuery,
                new { startDate, endDate });
        }




        public async Task<TransactionStatisticsResponse> GetAverageRevenuePerTransactionAsync(DateTime startDate, DateTime endDate)
        {
            const string avgRevenueQuery = @"
                SELECT 
                    FromAccountCurrency AS Currency,
                    AVG(FromAmount - Fee) AS AverageRevenue
                FROM [BankingSystem_db].[dbo].[Transactions]
                WHERE TransactionDate BETWEEN @startDate AND @endDate
                GROUP BY FromAccountCurrency;
                ";

            var avgRevenueResults = await _dataManager.Query<AverageRevenuePerTransactionByCurrencyDto, dynamic>(
                avgRevenueQuery,
                new { startDate, endDate }
            );

            var result = new TransactionStatisticsResponse
            {
                TransactionsInGEL = 0,
                TransactionsInUSD = 0,
                TransactionsInEUR = 0
            };

            foreach (var avg in avgRevenueResults)
            {
                switch (avg.Currency)
                {
                    case "GEL":
                        result.TransactionsInGEL = avg.AverageRevenue;
                        break;
                    case "USD":
                        result.TransactionsInUSD = avg.AverageRevenue;
                        break;
                    case "EUR":
                        result.TransactionsInEUR = avg.AverageRevenue;
                        break;
                }
            }

            return result;
        }
    }
}
