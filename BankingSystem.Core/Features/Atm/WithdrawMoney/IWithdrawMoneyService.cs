using BankingSystem.Core.Features.Atm.WithdrawMoney.Dto_s;

namespace BankingSystem.Core.Features.Atm.WithdrawMoney
{
	public interface IWithdrawMoneyService
	{
		Task<WithdrawResponseDto> WithdrawAsync(WithdrawRequestDto requestDto);
	}
}