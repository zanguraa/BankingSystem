﻿using BankingSystem.Core.Features.Atm.CardAuthorizations.Requests;
using BankingSystem.Core.Features.Cards;

namespace BankingSystem.Core.Features.Atm.CardAuthorization
{
	public interface ICardAuthorizationRepository
	{
		Task<Card> GetCardByNumberAsync(string cardNumber);
		Task<Card> GetCardFromRequestAsync(CardAuthorizationRequest request);
	}
}