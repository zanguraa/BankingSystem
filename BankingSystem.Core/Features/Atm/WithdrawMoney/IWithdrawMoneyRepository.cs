namespace BankingSystem.Core.Features.Atm.WithdrawMoney
{
	public interface IWithdrawMoneyRepository
	{
		Task<bool> RecordTransactionAsync(Transaction transaction);
		Task<bool> WithdrawAsync(int accountId, decimal amount, string currency);
		Task<bool> WithdrawAsync(string accountNumber, decimal amount);
	}
}