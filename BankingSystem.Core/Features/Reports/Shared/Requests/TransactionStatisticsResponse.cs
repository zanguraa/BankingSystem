namespace BankingSystem.Core.Features.Reports.Shared.Requests
{
    public class TransactionStatisticsResponse
    {
        public int TransactionsCount { get; set; }
        public decimal TransactionsInGEL { get; set; }
        public decimal TransactionsInUSD { get; set; }
        public decimal TransactionsInEUR { get; set; }
        public string Message { get; internal set; }
    }
}

