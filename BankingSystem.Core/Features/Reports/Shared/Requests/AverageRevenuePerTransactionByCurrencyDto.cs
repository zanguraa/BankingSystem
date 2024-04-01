namespace BankingSystem.Core.Features.Reports.Shared.Requests
{
    public class AverageRevenuePerTransactionByCurrencyDto
    {
        public string Currency { get; set; }
        public decimal AverageRevenue { get; set; }
    }
}
