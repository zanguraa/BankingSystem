using BankingSystem.Core.Features.Cards.CreateCard;

namespace BankingSystem.Core.Features.Cards
{
    public class CardService : ICardService
	{
		private readonly ICardRepository _cardRepository;
		private static readonly Random random = new Random();

		public CardService(ICardRepository cardRepository)
		{
			_cardRepository = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository));
		}

		public async Task<Card> CreateCardAsync(CreateCardRequest createCardRequest)
		{
			var UserInfo = await _cardRepository.GetUserFullNameById(createCardRequest.UserId);
			if (UserInfo == null) 
			{
				throw new Exception("user not found");
			}
			var card = new Card
			{
				CardNumber = GenerateCardNumber(16),
				FullName = UserInfo.FirstName + " " + UserInfo.LastName,
				ExpirationDate = createCardRequest.ExpirationDate,
				Cvv = createCardRequest.Cvv,
				Pin = createCardRequest.Pin.ToString(),
				MaxTried = createCardRequest.MaxTried,
				IsActive = true,
				IsLocked = false,
				CreatedAt = DateTime.UtcNow,
				UserId = createCardRequest.UserId,
				AccountId = createCardRequest.AccountId
			};
			var result = await _cardRepository.CreateCardAsync(card);
			return result;
		}	

		public async Task<List<Card>> GetCardsByUserIdAsync(int userId)
		{
			return await _cardRepository.GetCardsByUserIdAsync(userId);
		}

		private string GenerateCardNumber(int length)
		{
			char[] digits = new char[length];
			for (int i = 0; i < length; i++)
			{
				digits[i] = (char)(random.Next(10) + '0');
			}

			return new string(digits);
			
		}
	}
}
