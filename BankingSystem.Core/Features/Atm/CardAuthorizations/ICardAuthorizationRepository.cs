﻿using BankingSystem.Core.Features.Cards;

namespace BankingSystem.Core.Features.Atm.CardAuthorization
{
	public interface ICardAuthorizationRepository
	{
		Task<Card> GetCardByNumberAsync(string cardNumber);
		Task<bool> UpdatePinAsync(string cardNumber, string newPinHash);
		Task LogAuthorizationAttemptAsync(string cardNumber, bool isSuccess);
		Task<bool> IsCardActivatedAsync(string cardNumber);
	}
}