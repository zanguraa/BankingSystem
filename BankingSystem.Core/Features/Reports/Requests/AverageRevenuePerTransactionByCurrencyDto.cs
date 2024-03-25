namespace BankingSystem.Core.Features.Reports.Requests
{
    public class AverageRevenuePerTransactionByCurrencyDto
	{
		public string Currency { get; set; }
		public decimal AverageRevenue { get; set; }
	}
}
