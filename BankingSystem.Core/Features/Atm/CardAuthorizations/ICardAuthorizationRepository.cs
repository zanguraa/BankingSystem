namespace BankingSystem.Core.Features.Atm.CardAuthorization
{
	public interface ICardAuthorizationRepository
	{
		Task<BankAccount> GetBankAccountByCardNumberAsync(string cardNumber);
		Task<bool> UpdatePinCodeAsync(string cardNumber, string newPinHash);
		Task LogAuthorizationAttemptAsync(string cardNumber, bool isSuccess);
		Task<bool> IsCardActivatedAsync(string cardNumber);

	}
}