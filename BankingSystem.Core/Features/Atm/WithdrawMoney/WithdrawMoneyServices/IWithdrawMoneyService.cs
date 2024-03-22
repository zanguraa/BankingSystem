using BankingSystem.Core.Features.Atm.WithdrawMoney.Requests;

namespace BankingSystem.Core.Features.Atm.WithdrawMoney.WithdrawMoneyServices
{
    public interface IWithdrawMoneyService
    {
        Task<WithdrawResponse> WithdrawAsync(WithdrawRequestWithCardNumber requestDto);
    }
}