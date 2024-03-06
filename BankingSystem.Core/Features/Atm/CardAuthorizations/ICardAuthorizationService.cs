using Azure.Core;
using BankingSystem.Core.Features.Atm.CardAuthorizations.Dto_s;

namespace BankingSystem.Core.Features.Atm.CardAuthorization
{
	public interface ICardAuthorizationService
	{
		Task<bool> AuthorizeCardAsync(CardAuthorizationRequestDto request);

	}
}