﻿using BankingSystem.Core.Features.Atm.CardAuthorizations;
using BankingSystem.Core.Features.Atm.CardAuthorizations.Models.Requests;
using BankingSystem.Core.Features.Atm.Shared;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Core.Shared.Services;

public interface ICardAuthorizationService
{
    Task<string> AuthorizeCardAsync(CardAuthorizationRequest request);
}

public class CardAuthorizationService : ICardAuthorizationService
{
    private readonly ICardAuthorizationRepository _cardAuthorizationRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ICryptoService _cryptoService;
    private readonly ISeqLogger _seqLogger;

    public CardAuthorizationService(
        ICardAuthorizationRepository cardAuthorizationRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        ICryptoService cryptoService,
        ISeqLogger seqLogger
        )
    {
        _cardAuthorizationRepository = cardAuthorizationRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _cryptoService = cryptoService;
        _seqLogger = seqLogger;
    }

    public async Task<string> AuthorizeCardAsync(CardAuthorizationRequest request)
    {
        ValidateCardAuthorization(request);
        if (request == null) throw new ArgumentNullException(nameof(request));
        if (string.IsNullOrWhiteSpace(request.CardNumber) || request.CardNumber.Length != 16 || !request.CardNumber.All(char.IsDigit))
        {
            throw new InvalidCardException("Invalid card number.");
        }

        var encryptedPin = _cryptoService.Encrypt(request.Pin);

        var card = await _cardAuthorizationRepository.GetCardFromRequestAsync(request.CardNumber, encryptedPin);
        if (card == null)
        {
            throw new InvalidCardException($"CardNumber {request.CardNumber} not found.");
        }

        var jwtTokken = _jwtTokenGenerator.GenerateTokenForAtmOperations(request);
        _seqLogger.LogInfo("Card with CardNumber: {CardNumber} is authorized", request.CardNumber);
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
        if (request.CardNumber.Length != 16 && !request.CardNumber.All(char.IsDigit))
        {
            throw new InvalidCardException("CardNumber length is invalid!");
        }

        return true;
    }
}
