using BankingSystem.Core.Data.Entities;
using BankingSystem.Core.Features.Cards.CreateCard;

namespace BankingSystem.Core.Features.Cards
{
	public interface ICardService
	{
		Task<CardEntity> CreateCardAsync(CreateCardRequest createCardRequest);
		Task<CardEntity> GetCardByNumberAsync(string cardNumber);
		Task<List<CardEntity>> GetCardsByUserIdAsync(int userId);
	}
}