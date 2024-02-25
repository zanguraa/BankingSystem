using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.Core.Data.Entities;
using BankingSystem.Core.Features.Cards.CreateCard;

namespace BankingSystem.Core.Features.Cards
{
	public class CardService :  ICardService
	{
		private readonly ICardRepository _cardRepository;

		public CardService(ICardRepository cardRepository)
		{
			_cardRepository = cardRepository;
		}

		public async Task<CardEntity> CreateCardAsync(CreateCardRequest createCardRequest)
		{
			

			var card = new CardEntity
			{
				CardNumber = GenerateCardNumber(),
				FullName = createCardRequest.FullName,
				ExpirationDate = createCardRequest.ExpirationDate,
				Cvv = createCardRequest.Cvv,
				Pin = createCardRequest.Pin,
				MaxTried = createCardRequest.MaxTried,
				IsLocked = false,
				CreatedAt = DateTime.UtcNow,
				UserId = createCardRequest.UserId,  
				AccountId = createCardRequest.AccountId 
			};

			return await _cardRepository.CreateCardAsync(card);
		}

		public async Task<CardEntity> GetCardByNumberAsync(string cardNumber)
		{
			return await _cardRepository.GetCardByNumberAsync(cardNumber);
		}

		public async Task<List<CardEntity>> GetCardsByUserIdAsync(int userId)
		{
			return await _cardRepository.GetCardsByUserIdAsync(userId);
		}


		private string GenerateCardNumber()
		{
			
			return "1234567890123456";
		}
	}

}
