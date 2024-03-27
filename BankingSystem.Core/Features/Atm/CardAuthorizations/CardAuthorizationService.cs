using BankingSystem.Core.Features.Atm.CardAuthorizations.Requests;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Exceptions;

public interface ICardAuthorizationService
{
    Task<bool> AuthorizeCardAsync(CardAuthorizationRequest request);
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

    public async Task<bool> AuthorizeCardAsync(CardAuthorizationRequest request)
    {
        ValidateCardAuthorization(request);

        var card = await _cardAuthorizationRepository.GetCardFromRequestAsync(request);
        return card == null ? throw new InvalidCardException($"CardNumber {request.CardNumber} not found.") : true;
    }

    private bool ValidateCardAuthorization(CardAuthorizationRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        if (string.IsNullOrEmpty(request.CardNumber))
        {
            throw new InvalidCardException("CardNumber can not be empty");
        }
        if (request.Pin <= 0)
        {
            throw new InvalidCardException("invalid PinCode");
        }
        if (request.CardNumber.Length != 16 && !request.CardNumber.All(char.IsDigit))
        {
            throw new InvalidCardException("CardNumber length is invalid!");
        }

        return true;
    }
}
