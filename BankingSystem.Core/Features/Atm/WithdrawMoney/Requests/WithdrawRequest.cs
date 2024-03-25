namespace BankingSystem.Core.Features.Atm.WithdrawMoney.Requests
{
    public class WithdrawRequest
    {
        public int? AccountId { get; set; }
        public decimal Amount { get; set; } 
        public string Currency { get; set; } 
        public string CardNumber { get; set; }
        public decimal RequestedAmount { get; set; } 
        public string RequestedCurrency { get; set; } 
    }

}
