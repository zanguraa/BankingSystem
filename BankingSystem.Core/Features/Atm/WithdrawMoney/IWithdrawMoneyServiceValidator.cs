using BankingSystem.Core.Features.Atm.WithdrawMoney.Requests;

public interface IWithdrawMoneyServiceValidator
{
    void ValidateWithdrawRequest(WithdrawRequestWithCardNumber requestDto);
}