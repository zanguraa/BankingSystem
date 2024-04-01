namespace BankingSystem.Core.Features.Reports.Shared.Requests
{
    public class DailyTransactionCountDto
    {
        public DateTime Date { get; set; }
        public int TransactionCount { get; set; }
    }
}
