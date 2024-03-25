namespace BankingSystem.Core.Features.Atm.WithdrawMoney.Requests
{
    public class WithdrawnAmountByCurrencyDto
    {
        public string Currency { get; set; }
        public decimal TotalWithdrawn { get; set; }
    }
}
