using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.Core.Data;
using BankingSystem.Core.Data.Entities;
using BankingSystem.Core.Features.Cards.CreateCard;

namespace BankingSystem.Core.Features.Cards
{
	public class CardRepository : ICardRepository
	{
		private readonly IDataManager _dataManager;

		public CardRepository(IDataManager dataManager)
		{
			_dataManager = dataManager;
		}
		public async Task<Card?> GetCardByCardNumber(string CardNumber)
		{
			string query = "SELECT * FROM Cards WHERE CardNumber = @CardNumber";
			var result = await _dataManager.Query<Card, dynamic>(query, new { CardNumber });
			return result.FirstOrDefault();
		}
		public async Task<Card> GetCardByIdAsync(int cardId)
		{
			string query = "SELECT * FROM Cards WHERE Id = @CardId";
			var result = await _dataManager.Query<Card, dynamic>(query, new { CardId = cardId });
			return result.FirstOrDefault();
		}

		public async Task<Card> CreateCardAsync(Card card)
		{
			string query = @"
                INSERT INTO Cards (CardNumber, FullName, ExpirationDate, Cvv, Pin, MaxTried, isLocked, IsActive, CreatedAt, UserId, AccountId)
                VALUES (@CardNumber, @FullName, @ExpirationDate, @Cvv, @Pin, @MaxTried, @IsLocked, @IsActive, @CreatedAt, @UserId, @AccountId);";

			var result = await _dataManager.Execute(query, new
			{
				card.CardNumber,
				card.FullName,
				card.ExpirationDate,
				card.Cvv,
				card.Pin,
				card.MaxTried,
				card.IsLocked,
				card.IsActive,
				card.CreatedAt,
				card.UserId,
				card.AccountId
			});

			if (result == 0)
			{
				throw new Exception("Failed to create card");
			}

			var newCard = await GetCardByCardNumber(card.CardNumber);
			if (newCard == null)
			{
				throw new Exception("Failed to create card");
			}
			return newCard;
		}

		public async Task<List<Card>> GetCards()
		{
			string query = "SELECT Id, CardNumber, FullName, ExpirationDate, Cvv, Pin, MaxTried, isLocked, CreatedAt, UserId, AccountId FROM Cards";
			return (await _dataManager.Query<Card, dynamic>(query, null)).ToList();
		}

		public async Task<List<Card>> GetCardsByUserIdAsync(int userId) 
		{
			string query = @" SELECT * FROM Cards WHERE UserId = @userId";
			var result = await _dataManager.Query<Card,dynamic>(query, new {userId});
			return result.ToList();
		}
		public async Task<UserResponse?> GetUserFullNameById(int userId)
		{
			string query = @"SELECT FirstName, LastName FROM Users WHERE Id = @userId";
			var result = await _dataManager.Query<UserResponse, dynamic>(query, new { userId });
			return result.FirstOrDefault();
		}

	}
}
