namespace BankingSystem.Core.Features.Reports.Requests
{
    public class DailyTransactionCountDto
	{
        public DateTime Date { get; set; }
        public int TransactionCount { get; set; }
    }
}
