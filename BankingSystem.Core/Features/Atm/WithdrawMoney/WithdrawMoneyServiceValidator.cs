using BankingSystem.Core.Features.Atm.WithdrawMoney.Requests;
using BankingSystem.Core.Shared.Exceptions;

public class WithdrawMoneyServiceValidator : IWithdrawMoneyServiceValidator
{
    private readonly int _dailyWithdrawalLimitInGel = 10000;

    public void ValidateWithdrawRequest(WithdrawRequestWithCardNumber requestDto)
    {
        if (requestDto.Amount < 5 || requestDto.Amount % 5 != 0)
        {
            throw new InvalidAtmAmountException("Invalid withdrawal amount. Amount must be in multiples of 5");
        }
        if (requestDto.Amount > _dailyWithdrawalLimitInGel)
        {
            throw new InvalidAtmAmountException("Amount exceeds withdrawal limit");
        }

    }
}