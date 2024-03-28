using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Atm.CardAuthorizations.Models.Requests;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Core.Shared.Models;

namespace BankingSystem.Core.Features.Atm.CardAuthorizations;

public interface ICardAuthorizationRepository
{
    Task<Card> GetCardFromRequestAsync(CardAuthorizationRequest request);
}

public class CardAuthorizationRepository : ICardAuthorizationRepository
{
    private readonly IDataManager _dataManager;

    public CardAuthorizationRepository(IDataManager dataManager)
    {
        _dataManager = dataManager;
    }

    public async Task<Card> GetCardFromRequestAsync(CardAuthorizationRequest request)
    {
        var query = "SELECT * FROM Cards WHERE CardNumber = @CardNumber and Pin = @Pin";
        var result = await _dataManager.Query<Card, dynamic>(query, request);
        return result.FirstOrDefault();
    }
}