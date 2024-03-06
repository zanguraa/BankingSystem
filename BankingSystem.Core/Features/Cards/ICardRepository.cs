using BankingSystem.Core.Data.Entities;
using BankingSystem.Core.Features.Cards.CreateCard;

namespace BankingSystem.Core.Features.Cards
{
	public interface ICardRepository
	{
		Task<Card?> GetCardByCardNumber(string CardNumber);
		Task<Card> GetCardByIdAsync(int cardId);
		Task<Card> CreateCardAsync(Card card);
		Task<List<Card>> GetCards();
		Task<List<Card>> GetCardsByUserIdAsync(int userId);
		Task<UserResponse?> GetUserFullNameById(int userId);
	}
}