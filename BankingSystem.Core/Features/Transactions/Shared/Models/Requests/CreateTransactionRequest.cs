namespace BankingSystem.Core.Features.Transactions.Shared.Models.Requests
{
    public class CreateTransactionRequest
    {
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public required string Currency { get; set; }
        public required string ToCurrency { get; set; }
        public required string UserId { get; set; }
    }
}
