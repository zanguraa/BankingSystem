using BankingSystem.Core.Features.Atm.CardAuthorizations.Requests;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Exceptions;

public interface ICardAuthorizationService
{
    Task<string> AuthorizeCardAsync(CardAuthorizationRequest request);
}

public class CardAuthorizationService : ICardAuthorizationService
{
    private readonly ICardAuthorizationRepository _cardAuthorizationRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ISeqLogger _seqLogger;
	private string jwtTokken;

	public CardAuthorizationService(
        ICardAuthorizationRepository cardAuthorizationRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        ISeqLogger seqLogger)
    {
        _cardAuthorizationRepository = cardAuthorizationRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _seqLogger = seqLogger;
    }

    public async Task<string> AuthorizeCardAsync(CardAuthorizationRequest request)
    {
        ValidateCardAuthorization(request);
        //ეს დავამატე ტესტისთვის 
		if (request == null) throw new ArgumentNullException(nameof(request));
		if (string.IsNullOrWhiteSpace(request.CardNumber) || request.CardNumber.Length != 16 || !request.CardNumber.All(char.IsDigit))
		{
			throw new InvalidCardException("Invalid card number.");
		}


		var card = await _cardAuthorizationRepository.GetCardFromRequestAsync(request);
        if (card == null)
        {
            // If no card is found, throw a specific exception for this case.
            throw new InvalidCardException($"CardNumber {request.CardNumber} not found.");
        }

        _seqLogger.LogInfo("Card with CardNumber: {CardNumber} is authorized", request.CardNumber);
        // aq davamate fieldi ro errori momexsna da testi gameshva
        return (jwtTokken);
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
