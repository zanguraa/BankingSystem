using BankingSystem.Core.Features.Atm.CardAuthorizations.Requests;

namespace BankingSystem.Core.Features.Atm.CardAuthorization
{
    public interface ICardAuthorizationService
	{
        Task<bool> AuthorizeCardAsync(CardAuthorizationRequest request);


    }
}