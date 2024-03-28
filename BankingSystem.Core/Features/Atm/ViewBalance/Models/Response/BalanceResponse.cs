namespace BankingSystem.Core.Features.Atm.ViewBalance.Models.Response
{
    public class BalanceResponse
    {
        public string? UserId { get; set; }
        public decimal InitialAmount { get; set; }
        public string? Currency { get; set; }
    }
}
