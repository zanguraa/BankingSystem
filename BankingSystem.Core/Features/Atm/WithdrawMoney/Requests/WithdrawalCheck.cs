namespace BankingSystem.Core.Features.Atm.WithdrawMoney.Requests
{
    public class WithdrawalCheck
	{
		public int BankAccountId { get; set; }
		public DateTime WithdrawalDate { get; set; }
	}
}
