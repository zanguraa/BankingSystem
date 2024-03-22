using BankingSystem.Core.Features.Cards;

namespace BankingSystem.Core.Features.Atm.ChangePin
{
    public interface IChangePinRepository
	{
		Task<Card> GetCardByNumberAsync(string cardNumber);
		Task<bool> UpdatePinAsync(string cardNumber, string currentPin, string newPin);
	}
}
