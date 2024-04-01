namespace BankingSystem.Core.Features.Reports.Shared.Requests
{
    public class TotalWithdrawnAmountDto
    {
        public Dictionary<string, decimal> TotalWithdrawnAmountsByCurrency { get; set; } 
            = new Dictionary<string, decimal>();
    }
}
