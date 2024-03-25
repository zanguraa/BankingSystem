using BankingSystem.Core.Features.Atm.CardAuthorizations.Requests;
using BankingSystem.Core.Features.Cards;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Exceptions;

public interface ICardAuthorizationService
{
    Task<CardAuthorizationResponse> AuthorizeCardAsync(CardAuthorizationRequest request);
}

public class CardAuthorizationService : ICardAuthorizationService
{
    private readonly ICardAuthorizationRepository _cardAuthorizationRepository;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public CardAuthorizationService(
        ICardAuthorizationRepository cardAuthorizationRepository)
    {
        _cardAuthorizationRepository = cardAuthorizationRepository;
    }

    public async Task<CardAuthorizationResponse> AuthorizeCardAsync(CardAuthorizationRequest request)
    {
        await ValidateCardAuthorization(request);

        var card = await _cardAuthorizationRepository.GetCardFromRequestAsync(request);

        return new CardAuthorizationResponse { IsAuthorized = true, IsActive = card.IsActive, Message = "Message = \"Authorization successful.\"" };
    }

    private async Task<bool> ValidateCardAuthorization(CardAuthorizationRequest request)
    {
        var card = await _cardAuthorizationRepository.GetCardFromRequestAsync(request);
        var isCardValid = await _cardAuthorizationRepository.GetCardByNumberAsync(request.CardNumber);

        if (isCardValid.CardNumber != request.CardNumber)
        {
            throw new NotFoundException("Card not found!");
        }

        if (isCardValid.ExpirationDate < DateTime.UtcNow)
        {
            throw new ExpirationDateException("Card has expired.");
        }
        if (!isCardValid.IsActive)
        {
            throw new CardInactiveException("Card is not active!");
        }
        if (isCardValid.Pin != request.Pin)
        {
            throw new InvalidCardPinException("The provided PIN code is incorrect.");
        }

        return true;
    }
}
