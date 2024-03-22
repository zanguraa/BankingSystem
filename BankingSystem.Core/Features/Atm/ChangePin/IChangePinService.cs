namespace BankingSystem.Core.Features.Atm.ChangePin
{
    public interface IChangePinService
	{
		Task<bool> ChangePinAsync(string cardNumber, string currentPin, string newPin);
	}

}
