using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Reports.Requests;
using BankingSystem.Core.Features.Atm.WithdrawMoney.Requests;

namespace BankingSystem.Core.Features.Reports
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly IDataManager _dataManager;

        public ReportsRepository(IDataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public async Task<TransactionStatisticsDto> GetTransactionStatisticsAsync(DateTime startDate, DateTime endDate)
        {

            const string transactionQuery = @"
                SELECT 
                    COUNT(TransactionId) AS NumberOfTransactions,
                    SUM(CASE WHEN FromAccountCurrency = 'GEL' THEN FromAmount ELSE 0 END) AS IncomeLastMonthGEL,
                    SUM(CASE WHEN FromAccountCurrency = 'USD' THEN FromAmount ELSE 0 END) AS IncomeLastMonthUSD,
                    SUM(CASE WHEN FromAccountCurrency = 'EUR' THEN FromAmount ELSE 0 END) AS IncomeLastMonthEUR
                FROM [BankingSystem_db].[dbo].[Transactions]
                WHERE TransactionDate BETWEEN @startDate AND @endDate 
                ";


            var statisticsList = await _dataManager.Query<TransactionStatisticsAggregate, dynamic>(
                transactionQuery,
                new { startDate, endDate });

            var statistics = statisticsList.FirstOrDefault();

            if (statistics == null)
                return new TransactionStatisticsDto();

            return new TransactionStatisticsDto
            {
                TransactionsCount = statistics.NumberOfTransactions,
                IncomeGEL = statistics.IncomeLastMonthGEL,
                IncomeUSD = statistics.IncomeLastMonthUSD,
                IncomeEUR = statistics.IncomeLastMonthEUR
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

        public async Task<TotalWithdrawnAmountDto> GetTotalWithdrawnAmountAsync(DateTime startDate, DateTime endDate)
        {
            endDate = endDate.AddDays(1).AddSeconds(-1);

            const string withdrawalQuery = @"
                SELECT 
                  [FromAccountCurrency] AS Currency,
                  SUM([FromAmount]) AS TotalWithdrawn
                FROM [BankingSystem_db].[dbo].[Transactions]
                WHERE [TransactionDate] BETWEEN @startDate AND @endDate
                GROUP BY [FromAccountCurrency];
            ";

            var withdrawalAmounts = await _dataManager.Query<WithdrawnAmountByCurrencyDto, dynamic>(withdrawalQuery, new { startDate, endDate });

            var result = new TotalWithdrawnAmountDto();

            foreach (var withdrawal in withdrawalAmounts)
            {
                string currency = withdrawal.Currency;
                decimal totalWithdrawn = withdrawal.TotalWithdrawn;

                if (result.TotalWithdrawnAmountsByCurrency.ContainsKey(currency))
                {
                    result.TotalWithdrawnAmountsByCurrency[currency] += totalWithdrawn;
                }
                else
                {
                    result.TotalWithdrawnAmountsByCurrency.Add(currency, totalWithdrawn);
                }
            }

            return result;
        }


        public async Task<TransactionStatisticsDto> GetAverageRevenuePerTransactionAsync(DateTime startDate, DateTime endDate)
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

            var result = new TransactionStatisticsDto
            {
                IncomeGEL = 0,
                IncomeUSD = 0,
                IncomeEUR = 0
            };

            foreach (var avg in avgRevenueResults)
            {
                switch (avg.Currency)
                {
                    case "GEL":
                        result.IncomeGEL = avg.AverageRevenue;
                        break;
                    case "USD":
                        result.IncomeUSD = avg.AverageRevenue;
                        break;
                    case "EUR":
                        result.IncomeEUR = avg.AverageRevenue;
                        break;
                }
            }

            return result;
        }
        public async Task<UserStatisticsDto> GetUserStatisticsAsync()
        {
            var currentYearQuery = @"
                SELECT COUNT(*) 
                FROM Users 
                WHERE YEAR(RegisterDate) = YEAR(GETDATE());";

            var lastYearQuery = @"
                SELECT COUNT(*) 
                FROM Users 
                WHERE YEAR(RegisterDate) = YEAR(GETDATE()) - 1;";

            var last30DaysQuery = @"
                SELECT COUNT(*) 
                FROM Users 
                WHERE RegisterDate >= DATEADD(DAY, -30, GETDATE());";

            var currentYearCount = (await _dataManager.Query<int>(currentYearQuery)).FirstOrDefault();
            var lastYearCount = (await _dataManager.Query<int>(lastYearQuery)).FirstOrDefault();
            var last30DaysCount = (await _dataManager.Query<int>(last30DaysQuery)).FirstOrDefault();

            return new UserStatisticsDto
            {
                NumberOfUsersRegisteredCurrentYear = currentYearCount,
                NumberOfUsersRegisteredLastYear = lastYearCount,
                NumberOfUsersRegisteredLast30Days = last30DaysCount
            };
        }
    }
}
