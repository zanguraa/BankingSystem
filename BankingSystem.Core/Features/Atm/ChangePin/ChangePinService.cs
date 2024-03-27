
using BankingSystem.Core.Features.Atm.ChangePin.Requests;
using BankingSystem.Core.Shared.Exceptions;

namespace BankingSystem.Core.Features.Atm.ChangePin;

public interface IChangePinService
{
    Task<bool> ChangePinAsync(ChangePinRequest request);
}

public class ChangePinService : IChangePinService
{
    private readonly IChangePinRepository _changePinRepository;

    public ChangePinService(IChangePinRepository changePinRepository)
    {
        _changePinRepository = changePinRepository;
    }

    public async Task<bool> ChangePinAsync(ChangePinRequest request)
    {
        ValidateChangePinAsync(request);

        var card = await _changePinRepository.GetCardByNumberAsync(request.CardNumber);
        if (card == null || card.Pin != request.CurrentPin)
        {
            throw new InvalidCardException("Current Pin: {currentPin} is incorrect:!", request.CurrentPin);
        }

        return await _changePinRepository.UpdatePinAsync(request.CardNumber, request.CurrentPin, request.NewPin);
    }

    private static bool ValidateChangePinAsync(ChangePinRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CardNumber) || request.CardNumber.Length != 16 || !request.CardNumber.All(char.IsDigit))
        {
            throw new InvalidCardException("Invalid card number format: {cardNumber}.", request.CardNumber);
        }

        if (request.CurrentPin.ToString().Length != 4 || !request.CardNumber.ToString().All(char.IsDigit))
        {
            throw new InvalidCardException("Invalid current PIN format: {Pin} for card {Card}", request.CurrentPin, request.CardNumber);
        }

        if (request.NewPin.ToString().Length != 4 || !request.NewPin.ToString().All(char.IsDigit) || request.NewPin == request.CurrentPin)
        {
            throw new InvalidCardException("Invalid new PIN format or new PIN is the same as current PIN.");
        }
        return true;
    }
}