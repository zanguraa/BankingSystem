using System;
using System.Threading.Tasks;
using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Atm.WithdrawMoney;
using BankingSystem.Core.Features.Atm.WithdrawMoney.Dto_s; 
public class WithdrawMoneyService : IWithdrawMoneyService
{
	private readonly IWithdrawMoneyRepository _withdrawMoneyRepository;
	private readonly IBankAccountRepository _bankAccountRepository; // Assumed to check account details

	public WithdrawMoneyService(
		IWithdrawMoneyRepository withdrawMoneyRepository,
		IBankAccountRepository bankAccountRepository)
	{
		_withdrawMoneyRepository = withdrawMoneyRepository;
		_bankAccountRepository = bankAccountRepository;
	}

	public async Task<WithdrawResponseDto> WithdrawAsync(WithdrawRequestDto requestDto)
	{
		// Check for valid account and sufficient balance
		var account = await _bankAccountRepository.GetAccountByIdAsync(requestDto.AccountId);
		if (account == null)
		{
			return new WithdrawResponseDto { IsSuccessful = false, Message = "Account not found." };
		}

		if (account.InitialAmount < requestDto.Amount)
		{
			return new WithdrawResponseDto { IsSuccessful = false, Message = "Insufficient funds." };
		}

		// Attempt to perform the withdrawal
		bool success = await _withdrawMoneyRepository.WithdrawAsync(requestDto.AccountId, requestDto.Amount, requestDto.Currency);

		if (!success)
		{
			return new WithdrawResponseDto { IsSuccessful = false, Message = "Withdrawal failed due to a technical issue." };
		}

		// Assuming success and account balance is updated in the WithdrawAsync method
		return new WithdrawResponseDto
		{
			IsSuccessful = true,
			Message = $"Withdrawal of {requestDto.Amount} {requestDto.Currency} was successful.",
			RemainingBalance = account.InitialAmount - requestDto.Amount // Make sure the balance is updated in the database first
		};
	}
}