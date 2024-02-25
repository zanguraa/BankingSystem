using BankingSystem.Core.Data.Entities;
using BankingSystem.Core.Features.Cards.CreateCard;

namespace BankingSystem.Core.Features.Cards
{
	public interface ICardRepository
	{
		Task<CardEntity> CreateCardAsync(CardEntity card);
		Task<CardEntity> GetCardByIdAsync(int cardId);
		Task<CardEntity> GetCardByNumberAsync(string cardNumber);
		Task<List<CardEntity>> GetCardsByUserIdAsync(int userId);
	}
}