using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.Core.Data.Entities;
using Classroom.TodoWithAuth.Auth.Db;

namespace BankingSystem.Core.Features.Cards
{
	public class CardRepository : ICardRepository, ICardRepository
	{
		private readonly AppDbContext _dbContext;

		public CardRepository(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<CardEntity> CreateCardAsync(CardEntity card)
		{
			_dbContext.Cards.Add(card);
			await _dbContext.SaveChangesAsync();
			return card;
		}

		public async Task<CardEntity> GetCardByIdAsync(int cardId)
		{
			return await _dbContext.Cards.FindAsync(cardId);
		}

		public async Task<CardEntity> GetCardByNumberAsync(string cardNumber)
		{
			return await _dbContext.Cards.FirstOrDefaultAsync(c => c.CardNumber == cardNumber);
		}
	}
}
