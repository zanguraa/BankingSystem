using System;
using System.Threading.Tasks;
using BankingSystem.Core.Features.Atm.CardAuthorization;
using BankingSystem.Core.Data;

public class CardAuthorizationService : ICardAuthorizationService
{
	private readonly ICardAuthorizationRepository _cardAuthorizationRepository;

	public CardAuthorizationService(
		ICardAuthorizationRepository cardAuthorizationRepository)
	{
		_cardAuthorizationRepository = cardAuthorizationRepository;
	}

	public async Task<bool> AuthorizeCardAsync(string cardNumber, string pin)
	{
		var card = await _cardAuthorizationRepository.GetCardByNumberAsync(cardNumber);

		if (card == null || !card.IsActive || IsCardExpired(card.ExpirationDate))
		{
			return false;
		}
		return true;
	}

	private bool IsCardExpired(DateTime expirationDate)
	{
		return expirationDate < DateTime.UtcNow;
	}
}
