using BankingSystem.Core.Features.Atm.ChangePin.Models.Requests;
using BankingSystem.Core.Features.Atm.ChangePin.Models.Response;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Core.Shared.Services;

namespace BankingSystem.Core.Features.Atm.ChangePin;

public interface IChangePinService
{
    Task<ChangePinResponse> ChangePinAsync(ChangePinRequest request, string cardNumber);
}

public class ChangePinService : IChangePinService
{
    private readonly IChangePinRepository _changePinRepository;
    private readonly ISeqLogger _logger;
    private readonly ICryptoService _cryptoService;

    public ChangePinService(
        IChangePinRepository changePinRepository,
        ISeqLogger seqLogger,
        ICryptoService cryptoService
        )
    {
        _changePinRepository = changePinRepository;
        _logger = seqLogger;
        _cryptoService = cryptoService;
    }

    public async Task<ChangePinResponse> ChangePinAsync(ChangePinRequest request, string cardNumber)
    {
        request.CardNumber = cardNumber;
        ValidateChangePinAsync(request);

        var cryptedCurrentPin = _cryptoService.Encrypt(request.CurrentPin);
        var cryptedNewPin = _cryptoService.Encrypt(request.NewPin);

        var card = await _changePinRepository.GetCardByNumberAsync(request.CardNumber);
        if (card == null || card.Pin != cryptedCurrentPin)
        {
            throw new InvalidCardException("Current Pin: is incorrect:!");
        }

        await _changePinRepository.UpdatePinAsync(request.CardNumber, cryptedCurrentPin, cryptedNewPin);

        _logger.LogInfo("Pin was changed successfully for cardNumber: {cardNumber}", request.CardNumber);

        var result = new ChangePinResponse { Message = "Pin was changed successfully", Success = true };
        return result;
    }

    private static bool ValidateChangePinAsync(ChangePinRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CardNumber) || request.CardNumber.Length != 16 || !request.CardNumber.All(char.IsDigit))
        {
            throw new InvalidCardException("Invalid card number format: {cardNumber}.", request.CardNumber);
        }

        if (request.CurrentPin.Length != 4 || !request.CardNumber.All(char.IsDigit))
        {
            throw new InvalidCardException("Invalid current PIN format: for card {Card}", request.CardNumber);
        }

        if (request.NewPin.Length != 4 || !request.NewPin.All(char.IsDigit) || request.NewPin == request.CurrentPin)
        {
            throw new InvalidCardException("Invalid new PIN format or new PIN is the same as current PIN.");
        }
        return true;
    }
}