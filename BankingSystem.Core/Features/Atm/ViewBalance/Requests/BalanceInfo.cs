namespace BankingSystem.Core.Features.Atm.ViewBalance.Requests
{
    public class BalanceInfo
    {
        public string? UserId { get; set; }
        public decimal InitialAmount { get; set; }
        public string? Currency { get; set; }
    }
}
