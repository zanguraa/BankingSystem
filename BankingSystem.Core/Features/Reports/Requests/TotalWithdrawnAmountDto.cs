namespace BankingSystem.Core.Features.Reports.Requests
{
    public class TotalWithdrawnAmountDto
    {
        public Dictionary<string, decimal> TotalWithdrawnAmountsByCurrency { get; set; } = new Dictionary<string, decimal>();
    }
}
