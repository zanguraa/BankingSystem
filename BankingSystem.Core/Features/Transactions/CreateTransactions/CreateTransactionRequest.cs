namespace BankingSystem.Core.Features.Transactions.CreateTransactions
{
    public class CreateTransactionRequest
    {
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string ToCurrency { get; set; }
		public string? UserId { get; set; }
	}
}
