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

	public async Task<bool> UpdatePinAsync(string cardNumber, string newPinHash)
	{
		var query = "UPDATE Cards SET Pin = @NewPinHash WHERE CardNumber = @CardNumber";
		var result = await _dataManager.Execute(query, new { CardNumber = cardNumber, NewPinHash = newPinHash });
		return result > 0;
	}

	public async Task LogAuthorizationAttemptAsync(string cardNumber, bool isSuccess)
	{
		var query = "INSERT INTO AuthorizationAttempts (CardNumber, IsSuccess, AttemptDate) VALUES (@CardNumber, @IsSuccess, @AttemptDate)";
		await _dataManager.Execute(query, new { CardNumber = cardNumber, IsSuccess = isSuccess, AttemptDate = DateTime.UtcNow });
	}

	public async Task<bool> IsCardActivatedAsync(string cardNumber)
	{
		var query = "SELECT IsActive FROM Cards WHERE CardNumber = @CardNumber";
		var result = await _dataManager.Query<bool,dynamic>(query, new { CardNumber = cardNumber });
		return result.FirstOrDefault();
	}
}