using System;
using System.Collections.Generic;
using BankingSystem.Core.Features.Atm.ChangePin;

public class ChangePinService : IChangePinService
{
	private readonly IChangePinRepository _changePinRepository;

	public ChangePinService(IChangePinRepository changePinRepository)
	{
		_changePinRepository = changePinRepository;
	}

	public async Task<bool> ChangePinAsync(string cardNumber, string currentPin, string newPin)
	{
		// Validate the card's current PIN before proceeding with the update
		var card = await _changePinRepository.GetCardByNumberAsync(cardNumber);
		if (card == null || card.Pin != currentPin)
		{
			return false; // Card not found or current PIN is incorrect
		}

		// Update the PIN
		return await _changePinRepository.UpdatePinAsync(cardNumber, currentPin, newPin);
	}
}