using System.Transactions;
using BankingSystem.Core.Features.Atm.WithdrawMoney.Dto_s;

namespace BankingSystem.Core.Features.Atm.WithdrawMoney
{
	public interface IWithdrawMoneyRepository
	{
		Task<bool> RecordTransactionAsync(Transaction transaction);
		Task<bool> WithdrawAsync(WithdrawRequestDto request);
		Task<bool> WithdrawAsync(string accountNumber, decimal amount);
		Task<DecimalSum?> GetWithdrawalsOf24hoursByCardId(WithdrawalCheckDto options);

	}
}