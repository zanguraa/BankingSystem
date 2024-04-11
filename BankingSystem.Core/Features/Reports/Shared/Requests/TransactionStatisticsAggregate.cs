namespace BankingSystem.Core.Features.Reports.Shared.Requests
{
    public class TransactionStatisticsAggregate
    {
        public int NumberOfTransactions { get; set; }
        public decimal TransactionsInGEL { get; set; }
        public decimal TransactionsInUSD { get; set; }
        public decimal TransactionsInEUR { get; set; }
    }
}
