namespace BankingSystem.Core.Shared.Models
{
    public class BalanceInfo
    {
        public string? UserId { get; set; }
        public decimal InitialAmount { get; set; }
        public string? Currency { get; set; }
    }
}
