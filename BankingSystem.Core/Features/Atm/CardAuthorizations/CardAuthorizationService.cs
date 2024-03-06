using System;
using System.Threading.Tasks;
using BankingSystem.Core.Features.Atm.CardAuthorization;
using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Atm.CardAuthorizations.Dto_s;
using BankingSystem.Core.Features.Cards;

public class CardAuthorizationService : ICardAuthorizationService
{
	private readonly ICardAuthorizationRepository _cardAuthorizationRepository;

	public CardAuthorizationService(
		ICardAuthorizationRepository cardAuthorizationRepository)
	{
		_cardAuthorizationRepository = cardAuthorizationRepository;
	}

	public async Task<bool> AuthorizeCardAsync(CardAuthorizationRequestDto request)
	{
		var card = await _cardAuthorizationRepository.GetCardFromRequestAsync(request);
		return card != null && !IsCardExpired(card) && card.IsActive;
	}

	
	private bool IsCardExpired(Card card )
	{
		return card.ExpirationDate < DateTime.UtcNow;
	}
}
