using BankingSystem.Core.Features.Atm.WithdrawMoney.Requests;

namespace BankingSystem.Core.Features.Atm.WithdrawMoney
{
	public interface IWithdrawMoneyService
	{
		Task<WithdrawResponse> WithdrawAsync(WithdrawRequestWithCardNumber requestDto);
	}
}