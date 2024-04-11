using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Reports.Shared.Requests;

namespace BankingSystem.Core.Features.Reports.Withdrawals;

public interface IWithdrawalsRepository
{
    Task<TotalWithdrawnAmountDto> GetTotalWithdrawnAmountAsync(DateTime startDate, DateTime endDate);
}

public class WithdrawalsRepository : IWithdrawalsRepository
{
    private readonly IDataManager _dataManager;

    public WithdrawalsRepository(IDataManager dataManager)
    {
        _dataManager = dataManager;

    }

    public async Task<TotalWithdrawnAmountDto> GetTotalWithdrawnAmountAsync(DateTime startDate, DateTime endDate)
    {
        endDate = endDate.AddDays(1).AddSeconds(-1);

        const string withdrawalQuery = @"
                    SELECT 
                        ISNULL([ToAccountCurrency], [FromAccountCurrency]) AS Currency,
                        SUM(CASE 
                                WHEN [ToAccountCurrency] IS NOT NULL THEN [ToAmount]
                                ELSE [FromAmount] 
                            END) AS TotalWithdrawn
                    FROM 
                        [BankingSystem_db].[dbo].[Transactions]
                    WHERE 
                        [TransactionDate] BETWEEN @startDate AND @endDate 
                        AND [TransactionType] = 2
                    GROUP BY 
                        ISNULL([ToAccountCurrency], [FromAccountCurrency]);
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
}
