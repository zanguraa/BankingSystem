namespace BankingSystem.Core.Features.Reports.Requests
{
    public class TransactionStatisticsAggregate
	{
        public int NumberOfTransactions { get; set; }
        public decimal IncomeLastMonthGEL { get; set; }
        public decimal IncomeLastMonthUSD { get; set; }
        public decimal IncomeLastMonthEUR { get; set; }
    }
}
