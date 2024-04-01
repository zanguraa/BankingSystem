namespace BankingSystem.Core.Features.Reports.Shared.Requests
{
    public class WithdrawnAmountByCurrencyDto
    {
        public string Currency { get; set; }
        public decimal TotalWithdrawn { get; set; }
    }
}
