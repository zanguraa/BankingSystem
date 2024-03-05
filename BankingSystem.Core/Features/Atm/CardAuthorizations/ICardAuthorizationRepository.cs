using BankingSystem.Core.Features.Cards;

namespace BankingSystem.Core.Features.Atm.CardAuthorization
{
	public interface ICardAuthorizationRepository
	{
		Task<Card> GetCardByNumberAsync(string cardNumber);
	}
}