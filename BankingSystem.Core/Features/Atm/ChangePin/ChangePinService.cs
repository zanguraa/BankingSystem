
using BankingSystem.Core.Features.Atm.ChangePin.Requests;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Exceptions;

namespace BankingSystem.Core.Features.Atm.ChangePin;

public interface IChangePinService
{
    Task<ChangePinResponse> ChangePinAsync(ChangePinRequest request, string cardNumber);
}

public class ChangePinService : IChangePinService
{
    private readonly IChangePinRepository _changePinRepository;
    private readonly ISeqLogger _logger;

    public ChangePinService(IChangePinRepository changePinRepository, ISeqLogger seqLogger)
    {
        _changePinRepository = changePinRepository;
        _logger = seqLogger;
    }

    public async Task<ChangePinResponse> ChangePinAsync(ChangePinRequest request, string cardNumber)
    {
        request.CardNumber = cardNumber;
        ValidateChangePinAsync(request);

        var card = await _changePinRepository.GetCardByNumberAsync(request.CardNumber);
        if (card == null || card.Pin != request.CurrentPin)
        {
            throw new InvalidCardException("Current Pin: is incorrect:!");
        }

        await _changePinRepository.UpdatePinAsync(request.CardNumber, request.CurrentPin, request.NewPin);

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

        if (request.CurrentPin.ToString().Length != 4 || !request.CardNumber.ToString().All(char.IsDigit))
        {
            throw new InvalidCardException("Invalid current PIN format: for card {Card}", request.CardNumber);
        }

        if (request.NewPin.ToString().Length != 4 || !request.NewPin.ToString().All(char.IsDigit) || request.NewPin == request.CurrentPin)
        {
            throw new InvalidCardException("Invalid new PIN format or new PIN is the same as current PIN.");
        }
        return true;
    }
}