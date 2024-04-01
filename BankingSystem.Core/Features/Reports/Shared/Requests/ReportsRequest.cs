namespace BankingSystem.Core.Features.Reports.Shared.Requests
{
    public class ReportsRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } = DateTime.MinValue;
    }
}
