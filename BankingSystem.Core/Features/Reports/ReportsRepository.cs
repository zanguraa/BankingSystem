using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Reports.Dto_s;

namespace BankingSystem.Core.Features.Reports
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly IDataManager _dataManager;

        public ReportsRepository(IDataManager dataManager)
        {
            _dataManager = dataManager;
        }

        // Gets statistical data for transactions within a given time frame
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


            // Use QueryAsync and then FirstOrDefault to get a single instance of TransactionStatisticsAggregate
            var statisticsList = await _dataManager.Query<TransactionStatisticsAggregate, dynamic>(
                transactionQuery,
                new { startDate, endDate });
            var statistics = statisticsList.FirstOrDefault(); // This is where we mimic QueryFirstOrDefaultAsync

            if (statistics == null) return new TransactionStatisticsDto();

            // Map the aggregated result to the DTO
            return new TransactionStatisticsDto
            {
                TransactionsLastMonth = statistics.NumberOfTransactions,
                IncomeLastMonthGEL = statistics.IncomeLastMonthGEL,
                IncomeLastMonthUSD = statistics.IncomeLastMonthUSD,
                IncomeLastMonthEUR = statistics.IncomeLastMonthEUR,
                // Initialize other properties as needed
            };
        }
        // Gets daily transaction counts within a given time frame
        public async Task<IEnumerable<DailyTransactionCountDto>> GetDailyTransactionCountsAsync(DateTime startDate, DateTime endDate)
        {
            const string dailyCountQuery = @"
                SELECT 
                    CAST(TransactionDate AS DATE) AS Date, 
                    COUNT(*) AS TransactionCount
                FROM Transactions
                WHERE TransactionDate BETWEEN @startDate AND @endDate
                GROUP BY CAST(TransactionDate AS DATE)
                ORDER BY Date;
            ";

            return await _dataManager.Query<DailyTransactionCountDto, dynamic>(
                dailyCountQuery,
                new { startDate, endDate });
        }
        public async Task<TotalWithdrawnAmountDto> GetTotalWithdrawnAmountAsync(DateTime startDate, DateTime endDate)
        {
            const string withdrawalQuery = @"
    SELECT 
        Currency,
        SUM(TotalAmount) AS TotalWithdrawn
    FROM [BankingSystem_db].[dbo].[DailyWithdrawals]
    WHERE WithdrawalDate BETWEEN @startDate AND @endDate
    GROUP BY Currency;
";


            var withdrawalAmounts = await _dataManager.Query<WithdrawnAmountByCurrencyDto, dynamic>(withdrawalQuery, new { startDate, endDate });

            var result = new TotalWithdrawnAmountDto();

            // Initialize totals to 0 to ensure all currencies are accounted for.
            result.TotalWithdrawnAmountGEL = 0;
            result.TotalWithdrawnAmountUSD = 0;
            result.TotalWithdrawnAmountEUR = 0;

            foreach (var withdrawal in withdrawalAmounts)
            {
                switch (withdrawal.Currency)
                {
                    case "GEL":
                        result.TotalWithdrawnAmountGEL = withdrawal.TotalWithdrawn;
                        break;
                    case "USD":
                        result.TotalWithdrawnAmountUSD = withdrawal.TotalWithdrawn;
                        break;
                    case "EUR":
                        result.TotalWithdrawnAmountEUR = withdrawal.TotalWithdrawn;
                        break;
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
                // Initialize all properties to 0 or appropriate default values
                AverageRevenuePerTransactionGEL = 0,
                AverageRevenuePerTransactionUSD = 0,
                AverageRevenuePerTransactionEUR = 0
            };

            foreach (var avg in avgRevenueResults)
            {
                switch (avg.Currency)
                {
                    case "GEL":
                        result.AverageRevenuePerTransactionGEL = avg.AverageRevenue;
                        break;
                    case "USD":
                        result.AverageRevenuePerTransactionUSD = avg.AverageRevenue;
                        break;
                    case "EUR":
                        result.AverageRevenuePerTransactionEUR = avg.AverageRevenue;
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
        WHERE YEAR(RegistrationDate) = YEAR(GETDATE());";

            var lastYearQuery = @"
        SELECT COUNT(*) 
        FROM Users 
        WHERE YEAR(RegistrationDate) = YEAR(GETDATE()) - 1;";

            var last30DaysQuery = @"
        SELECT COUNT(*) 
        FROM Users 
        WHERE RegistrationDate >= DATEADD(DAY, -30, GETDATE());";

            // Use Query method and manually extract the first or default value
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
