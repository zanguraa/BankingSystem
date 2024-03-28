namespace BankingSystem.Core.Features.Atm.WithdrawMoney.Requests
{
    public class WithdrawRequestWithCardNumber : WithdrawAmountCurrencyRequest
    {
        public string CardNumber { get; set; }


    }
}
