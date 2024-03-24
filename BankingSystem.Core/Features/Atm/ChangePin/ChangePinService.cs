
namespace BankingSystem.Core.Features.Atm.ChangePin;

public interface IChangePinService
{
    Task<bool> ChangePinAsync(string cardNumber, string currentPin, string newPin);
}

public class ChangePinService : IChangePinService
{
    private readonly IChangePinRepository _changePinRepository;

    public ChangePinService(IChangePinRepository changePinRepository)
    {
        _changePinRepository = changePinRepository;
    }

    public async Task<bool> ChangePinAsync(string cardNumber, string currentPin, string newPin)
    {
        var card = await _changePinRepository.GetCardByNumberAsync(cardNumber);
        if (card == null || card.Pin != currentPin)
        {
            return false;
        }

        return await _changePinRepository.UpdatePinAsync(cardNumber, currentPin, newPin);
    }
}