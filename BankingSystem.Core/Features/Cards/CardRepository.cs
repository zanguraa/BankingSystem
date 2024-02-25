using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.Core.Data;
using BankingSystem.Core.Data.Entities;
using BankingSystem.Core.Features.Cards.CreateCard;
using Classroom.TodoWithAuth.Auth.Db;

namespace BankingSystem.Core.Features.Cards
{
	public class CardRepository : ICardRepository
	{
		private readonly IDataManager _dataManager;

		public CardRepository(IDataManager dataManager)
		{
			_dataManager = dataManager;
		}

		public async Task<CardEntity?> GetCardByIdAsync(int cardId)
		{
			string query = "SELECT * FROM Cards WHERE Id = @CardId";
			var result = await _dataManager.Query<CardEntity, dynamic>(query, new { CardId = cardId });
			return result.FirstOrDefault();
		}

		public async Task<int> CreateCardAsync(CardEntity card)
		{
			string query = @"
                INSERT INTO Cards (CardNumber, FullName, ExpirationDate, Cvv, Pin, MaxTried, isLocked, CreatedAt, UserId, AccountId)
                VALUES (@CardNumber, @FullName, @ExpirationDate, @Cvv, @Pin, @MaxTried, @IsLocked, @CreatedAt, @UserId, @AccountId);";

			var result = await _dataManager.Execute(query, new
			{
				card.CardNumber,
				card.FullName,
				card.ExpirationDate,
				card.Cvv,
				card.Pin,
				card.MaxTried,
				card.IsLocked,
				card.CreatedAt,
				card.UserId,
				card.AccountId
			});

			if (result == 0)
			{
				throw new Exception("Failed to create card");
			}

			var newCard = await GetCardByIdAsync(card.Id);
			if (newCard == null)
			{
				throw new Exception("Failed to create card");
			}
			return newCard.Id;
		}

		public async Task<List<CardEntity>> GetCards()
		{
			string query = "SELECT Id, CardNumber, FullName, ExpirationDate, Cvv, Pin, MaxTried, isLocked, CreatedAt, UserId, AccountId FROM Cards";
			return (await _dataManager.Query<CardEntity, dynamic>(query, null)).ToList();
		}

		Task<CardEntity> ICardRepository.CreateCardAsync(CardEntity card)
		{
			throw new NotImplementedException();
		}

		public Task<CardEntity> GetCardByNumberAsync(string cardNumber)
		{
			throw new NotImplementedException();
		}

		public Task<List<CardEntity>> GetCardsByUserIdAsync(int userId)
		{
			throw new NotImplementedException();
		}

		public Task<CardEntity> CreateCardAsync(CreateCardRequest cardRequest)
		{
			throw new NotImplementedException();
		}
	}

}
