using BankingSystem.Core.Features.BankAccounts.CreateAccount;
using BankingSystem.Core.Features.Cards.CreateCard;
using BankingSystem.Core.Features.Users;
using BankingSystem.Core.Shared.Exceptions;

namespace BankingSystem.Core.Features.Cards
{
    public interface ICardService
    {
        Task<Card> CreateCardAsync(CreateCardRequest createCardRequest);
        Task<List<Card>> GetCardsByUserIdAsync(int userId);
    }

    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
        private readonly ICreateBankAccountsRepository _createBankAccountsRepository;
        private readonly IUserRepository _userRepository;
        private static readonly Random random = new Random();

        public CardService(ICardRepository cardRepository, ICreateBankAccountsRepository createBankAccountsRepository, IUserRepository userRepository)
        {
            _cardRepository = cardRepository;
            _createBankAccountsRepository = createBankAccountsRepository;
            _userRepository = userRepository;
        }

        public async Task<Card> CreateCardAsync(CreateCardRequest createCardRequest)
        {
            await CreateCardValidation(createCardRequest);

            var UserInfo = await _cardRepository.GetUserFullNameById(createCardRequest.UserId);
            
            var card = new Card
            {
                CardNumber = GenerateCardNumber(16),
                FullName = UserInfo.FirstName + " " + UserInfo.LastName,
                ExpirationDate = createCardRequest.ExpirationDate,
                Cvv = createCardRequest.Cvv,
                Pin = createCardRequest.Pin,
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

        private async Task CreateCardValidation(CreateCardRequest createCardRequest)
        {
            if (createCardRequest.UserId <= 0 || !await _userRepository.UserExistsAsync(createCardRequest.UserId))
            {
                throw new UserNotFoundException("Invalid User ID or User does not exist.");
            }

            if (createCardRequest.AccountId <= 0)
            {
                throw new BankAccountNotFoundException("Invalid Account ID or Account does not exist.");
            }
            if (await _createBankAccountsRepository.GetAccountByIdAsync(createCardRequest.AccountId) == null)
            {
                throw new BankAccountNotFoundException("Invalid Account ID or Account does not exist.");
            }

            if (createCardRequest.ExpirationDate <= DateTime.UtcNow)
            {
                throw new InvalidExpirationDateException("Expiration date must be in the future.");
            }

            if (createCardRequest.Cvv < 100 || createCardRequest.Cvv > 999)
            {
                throw new InvalidCvvFormatException("CVV must be a 3-digit number.");
            }

            if (createCardRequest.Pin < 1000 || createCardRequest.Pin > 9999)
            {
                throw new InvalidPinFormatException("PIN must be a 4-digit number.");
            }

            if (createCardRequest.MaxTried <= 0)
            {
                throw new InvalidMaxTriedValueException("Max Tried must be a positive number.");
            }

        }
    }
}
