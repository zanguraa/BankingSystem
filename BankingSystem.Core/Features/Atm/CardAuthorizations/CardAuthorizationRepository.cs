using BankingSystem.Core.Data;
using BankingSystem.Core.Shared.Models;

namespace BankingSystem.Core.Features.Atm.CardAuthorizations;

public interface ICardAuthorizationRepository
{
    Task<Card> GetCardFromRequestAsync(string cardNumber, string hashedPin);
    Task UpdateCardStatusAsync(int cardId, bool isActive);
}

public class CardAuthorizationRepository : ICardAuthorizationRepository
{
    private readonly IDataManager _dataManager;

    public CardAuthorizationRepository(IDataManager dataManager)
    {
        _dataManager = dataManager;
    }

    public async Task<Card> GetCardFromRequestAsync(string cardNumber, string hashedPin)
    {

        var query = "SELECT * FROM Cards WHERE CardNumber = @CardNumber and Pin = @Pin";
        var result = await _dataManager.Query<Card, dynamic>(query, new { CardNumber = cardNumber, Pin = hashedPin});
        return result.FirstOrDefault();
    }

    public async Task UpdateCardStatusAsync(int cardId, bool isActive)
    {
        var query = "UPDATE Cards SET IsActive = @IsActive WHERE Id = @CardId";
        await _dataManager.Execute(query, new { IsActive = isActive, CardId = cardId });
    }
}