﻿using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Atm.CardAuthorization;
using BankingSystem.Core.Features.Atm.CardAuthorizations.Dto_s;
using BankingSystem.Core.Features.Cards;

public class CardAuthorizationRepository : ICardAuthorizationRepository
{
	private readonly IDataManager _dataManager;

	public CardAuthorizationRepository(IDataManager dataManager)
	{
		_dataManager = dataManager;
	}

	public async Task<Card> GetCardByNumberAsync(string CardNumber)
	{
		var query = "SELECT * FROM Cards WHERE CardNumber = @CardNumber";
		var result = await _dataManager.Query<Card,dynamic>(query, new { CardNumber });
		return result.FirstOrDefault();
	}
	public async Task<Card> GetCardFromRequestAsync(CardAuthorizationRequestDto request)
	{
		var query = "SELECT * FROM Cards WHERE CardNumber = @CardNumber and Pin = @Pin";
		var result = await _dataManager.Query<Card, dynamic>(query,request);
		return result.FirstOrDefault();
	}
}