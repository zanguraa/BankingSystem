using BankingSystem.Core.Features.Cards.CreateCard;

namespace BankingSystem.Core.Features.Cards
{
    public interface ICardService
	{
		Task<Card> CreateCardAsync(CreateCardRequest createCardRequest);
		Task<List<Card>> GetCardsByUserIdAsync(int userId);
	}
}