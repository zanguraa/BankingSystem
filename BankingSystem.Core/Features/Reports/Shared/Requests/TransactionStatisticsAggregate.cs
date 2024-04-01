namespace BankingSystem.Core.Features.Reports.Shared.Requests
{
    public class TransactionStatisticsAggregate
    {
        public int NumberOfTransactions { get; set; }
        public decimal IncomeLastMonthInGEL { get; set; }
        public decimal IncomeLastMonthInUSD { get; set; }
        public decimal IncomeLastMonthInEUR { get; set; }
    }
}
