namespace BankingSystem.Core.Features.Atm.WithdrawMoney.Models.Requests
{
    public class WithdrawalCheck
    {
        public int BankAccountId { get; set; }
        public DateTime WithdrawalDate { get; set; }
    }
}
