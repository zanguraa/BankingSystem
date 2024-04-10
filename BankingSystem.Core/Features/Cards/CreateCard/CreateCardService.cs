using BankingSystem.Core.Features.BankAccounts.CreateAccount;
using BankingSystem.Core.Features.Cards.CreateCard.Models.Requests;
using BankingSystem.Core.Features.Cards.CreateCard.Models.Response;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Core.Shared.Models;
using System.Text;

namespace BankingSystem.Core.Features.Cards.CreateCard
{
    public interface ICardService
    {
        Task<CardDto> CreateCardAsync(CreateCardRequest createCardRequest);
        Task<List<Card>> GetCardsByUserIdAsync(int userId);
    }

    public class CreateCardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
        private readonly ICreateBankAccountsRepository _createBankAccountsRepository;
        private readonly ICryptoService _cryptoService;
        private readonly ISeqLogger _seqLogger;

        public CreateCardService(
            ICardRepository cardRepository,
            ICreateBankAccountsRepository createBankAccountsRepository,
            ISeqLogger seqLogger,
            ICryptoService cryptoService
            )
        {
            _cardRepository = cardRepository;
            _createBankAccountsRepository = createBankAccountsRepository;
            _cryptoService = cryptoService;
            _seqLogger = seqLogger;
        }

        public async Task<CardDto> CreateCardAsync(CreateCardRequest createCardRequest)
        {
            var validationResult = await CreateCardValidationAsync(createCardRequest);

            var currentTime = DateTime.UtcNow;
            var pin = GenerateNumbers(4).ToString();

            var card = new Card
            {
                CardNumber = GenerateNumbers(16).ToString(),
                FullName = validationResult.User.FirstName.ToUpper() + " " + validationResult.User.LastName.ToUpper(),
                ExpirationDate = new DateTime(currentTime.Year, currentTime.Month, 1).AddYears(2).AddMonths(1).AddDays(-1),
                Cvv = int.Parse(GenerateNumbers(3)),
                Pin = _cryptoService.Encrypt(pin),
                UserId = createCardRequest.UserId,
                AccountId = createCardRequest.AccountId
            };

            var result = await _cardRepository.CreateCardAsync(card);

            _seqLogger.LogInfo("Created Card: {CardNumber} for UserId {UserId}", card.CardNumber, createCardRequest.UserId);

            return result.ToDto(pin);
        }

        public async Task<List<Card>> GetCardsByUserIdAsync(int userId)
        {
            return await _cardRepository.GetCardsByUserIdAsync(userId);
        }

        private string GenerateNumbers(int length)
        {
            if (length == 0)
            {
                throw new ArgumentException("length parameter is out of range");
            }
            StringBuilder sb = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                sb.Append(random.Next(0, 9));
            }
            return sb.ToString();
        }

        private async Task<ValidatedCardData> CreateCardValidationAsync(CreateCardRequest createCardRequest)
        {
            if (createCardRequest.UserId <= 0)
            {
                throw new UserNotFoundException("Invalid User ID.{userId}", createCardRequest.UserId);
            }
            if (createCardRequest.AccountId <= 0)
            {
                throw new BankAccountNotFoundException("Invalid Account ID.{BankAccountId}", createCardRequest.AccountId);
            }

            var UserInfo = await _cardRepository.GetUserFullNameByIdAsync(createCardRequest.UserId)
                ?? throw new UserNotFoundException("User not found {userId}", createCardRequest.UserId);
            var bankAccount = await _createBankAccountsRepository.GetAccountByIdAsync(createCardRequest.AccountId)
                ?? throw new BankAccountNotFoundException("BankAcount not found. {BankAccountId}", createCardRequest.AccountId);
            return new() { User = UserInfo, BankAccount = bankAccount };
        }

        public class ValidatedCardData
        {
            public UserResponse User { get; set; }
            public BankAccount BankAccount { get; set; }
        }
    }
}
