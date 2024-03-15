using System.Transactions;
using BankingSystem.Core.Features.Atm.WithdrawMoney.Requests;

namespace BankingSystem.Core.Features.Atm.WithdrawMoney
{
	public interface IWithdrawMoneyRepository
	{
		Task<bool> RecordTransactionAsync(Transaction transaction);
		Task<bool> WithdrawAsync(WithdrawRequest request);
		Task<bool> WithdrawAsync(string accountNumber, decimal amount);
		Task<DecimalSum?> GetWithdrawalsOf24hoursByCardId(WithdrawalCheck options);

	}
}