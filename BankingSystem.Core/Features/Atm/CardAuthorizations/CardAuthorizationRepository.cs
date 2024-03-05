using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Atm.CardAuthorization;
using BankingSystem.Core.Features.Cards;

public class CardAuthorizationRepository : ICardAuthorizationRepository
{
	private readonly IDataManager _dataManager;

	public CardAuthorizationRepository(IDataManager dataManager)
	{
		_dataManager = dataManager;
	}

	public async Task<Card> GetCardByNumberAsync(string cardNumber)
	{
		var query = "SELECT * FROM Cards WHERE CardNumber = @CardNumber";
		var result = await _dataManager.Query<Card,dynamic>(query, new { CardNumber = cardNumber });
		return result.FirstOrDefault();
	}
}