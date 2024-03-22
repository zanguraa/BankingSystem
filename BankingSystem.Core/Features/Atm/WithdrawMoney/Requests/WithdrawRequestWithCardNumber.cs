namespace BankingSystem.Core.Features.Atm.WithdrawMoney.Requests
{
    public class WithdrawRequestWithCardNumber : WithdrawAmountCurrency
    {
        public string CardNumber { get; set; }


    }
}
