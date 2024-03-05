namespace BankingSystem.Core.Features.Atm.CardAuthorization
{
	public interface ICardAuthorizationService
	{
		Task<bool> AuthorizeCardAsync(string cardNumber, string pin);

	}
}