using System.Threading.Tasks;
using Azure.Core;
using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Atm.WithdrawMoney.Requests;
using BankingSystem.Core.Features.BankAccounts;

namespace BankingSystem.Core.Features.Atm.WithdrawMoney
{
	public class WithdrawMoneyService : IWithdrawMoneyService
	{
		private readonly IWithdrawMoneyRepository _withdrawMoneyRepository;
		private readonly IBankAccountRepository _bankAccountRepository;
		private readonly int _dailyWithdrawalLimitInGel = 10000;
		public WithdrawMoneyService(IWithdrawMoneyRepository withdrawMoneyRepository, IBankAccountRepository bankAccountRepository)
		{
			_withdrawMoneyRepository = withdrawMoneyRepository;
			_bankAccountRepository = bankAccountRepository;
		}

		public async Task<WithdrawResponse> WithdrawAsync(WithdrawRequest requestDto)
		{
			var account = await _bankAccountRepository.GetAccountByIdAsync(requestDto.AccountId);
			var commission = requestDto.Amount * 0.02m;
			var DeductAmount  = commission + requestDto.Amount;
			if (account == null)
			{
				return new WithdrawResponse { IsSuccessful = false, Message = "Account not found." };
			}

			if (!Enum.TryParse<CurrencyType>(requestDto.Currency, out var requestedCurrency))
			{
				return new WithdrawResponse { IsSuccessful = false, Message = "Invalid currency specified." };
			}

			if (account.InitialAmount < requestDto.Amount || account.Currency != requestedCurrency)
			{
				return new WithdrawResponse { IsSuccessful = false, Message = "Insufficient funds or currency mismatch.", RemainingBalance = account.InitialAmount };
			}	
			
			if (DeductAmount >= account.InitialAmount ) 
			{
				return new WithdrawResponse { IsSuccessful = false, Message = "Insufficient Balancer Or Bad Request.", RemainingBalance = account.InitialAmount };
			}

			////ახლიდან ვიძახებ account რო მივიღო განახლებული ბალანსი
			account = await _bankAccountRepository.GetAccountByIdAsync(requestDto.AccountId);

		
			
			
			WithdrawalCheck withdrawalCheckDto = new() { 
				BankAccountId = requestDto.AccountId,
				WithdrawalDate = DateTime.Now.AddDays(-1),
			};

			var TotalWithdrawedAmountInGel = await _withdrawMoneyRepository.GetWithdrawalsOf24hoursByCardId(withdrawalCheckDto);

			if (TotalWithdrawedAmountInGel == null) 
			{
		         return new WithdrawResponse { IsSuccessful = false, Message = "Server Error. Failed To Get Withdrawal Data  " };

			}

			if (TotalWithdrawedAmountInGel.Sum >= _dailyWithdrawalLimitInGel) 
			{
				return new WithdrawResponse { IsSuccessful = false, Message = "24 hours limit reached." , RemainingBalance = account.InitialAmount };

			}
			WithdrawRequest withdrawRequest = new() { 
				AccountId = requestDto.AccountId,
				Currency= requestDto.Currency,
				Amount = DeductAmount,
			};


			var result = await _withdrawMoneyRepository.WithdrawAsync(withdrawRequest);

			return new WithdrawResponse
			{
				IsSuccessful = true,
				Message = $"Withdrawal of {requestDto.Amount} {requestDto.Currency} was successful.",
				RemainingBalance = account.InitialAmount - DeductAmount,
			};
		}
	}
}