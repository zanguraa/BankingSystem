using BankingSystem.Core.Data.Entities;

namespace BankingSystem.Core.Features.Cards
{
	public interface ICardRepository
	{
		Task<CardEntity> CreateCardAsync(CardEntity card);
		Task<CardEntity> GetCardByIdAsync(int cardId);
		Task<CardEntity> GetCardByNumberAsync(string cardNumber);
	}
}