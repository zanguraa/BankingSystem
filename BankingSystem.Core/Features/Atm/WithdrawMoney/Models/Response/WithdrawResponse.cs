namespace BankingSystem.Core.Features.Atm.WithdrawMoney.Models.Response
{
    public class WithdrawResponse
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public decimal RemainingBalance { get; set; }
        public decimal Commission { get; set; }
        public decimal RequestedAmount { get; set; }
        public string RequestedCurrency { get; set; }
        public decimal DeductedAmount { get; set; }
        public string AccountCurrency { get; set; }
        public DateTime WithdrawalDate { get; set; }

    }
}
