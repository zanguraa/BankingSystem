namespace BankingSystem.Core.Features.Transactions
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public string FromAccountCurrency { get; set; }
        public string ToAccountCurrency { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public decimal Fee { get; set; }
        public int TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
    }

}
