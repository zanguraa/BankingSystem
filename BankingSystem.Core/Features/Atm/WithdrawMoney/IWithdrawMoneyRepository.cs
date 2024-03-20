using System.Transactions;
using BankingSystem.Core.Features.Atm.WithdrawMoney.Requests;

namespace BankingSystem.Core.Features.Atm.WithdrawMoney
{
	public interface IWithdrawMoneyRepository
	{
		Task<DecimalSum?> GetWithdrawalsOf24hoursByCardId(WithdrawalCheck options);
	}
}