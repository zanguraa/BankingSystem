using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Atm.CardAuthorizations.Requests;
using BankingSystem.Core.Features.Cards;
using BankingSystem.Core.Shared.Exceptions;

public interface ICardAuthorizationRepository
{
    Task<Card> GetCardByNumberAsync(string CardNumber);
    Task<Card> GetCardFromRequestAsync(CardAuthorizationRequest request);
}

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
        var result = await _dataManager.Query<Card, dynamic>(query, new { CardNumber })
            ?? throw new InvalidCardException("card not found {CardNumber}", CardNumber);

        return result.FirstOrDefault();
    }
    public async Task<Card> GetCardFromRequestAsync(CardAuthorizationRequest request)
    {
        var query = "SELECT * FROM Cards WHERE CardNumber = @CardNumber and Pin = @Pin";
        var result = await _dataManager.Query<Card, dynamic>(query, request);
        return result.FirstOrDefault();
    }
}