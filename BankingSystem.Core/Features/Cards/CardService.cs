using BankingSystem.Core.Features.BankAccounts.CreateAccount;
using BankingSystem.Core.Features.BankAccounts.Requests;
using BankingSystem.Core.Features.Cards.CreateCard;
using BankingSystem.Core.Features.Users;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Exceptions;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Text;

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
        private readonly ISeqLogger _seqLogger;

        public CardService(ICardRepository cardRepository, ICreateBankAccountsRepository createBankAccountsRepository, ISeqLogger seqLogger)
        {
            _cardRepository = cardRepository;
            _createBankAccountsRepository = createBankAccountsRepository;
            _seqLogger = seqLogger;
        }

        public async Task<Card> CreateCardAsync(CreateCardRequest createCardRequest)
        {
            var validationResult = await CreateCardValidation(createCardRequest);

            var currentTime = DateTime.UtcNow;

            var card = new Card
            {
                CardNumber = GenerateNumbers(16).ToString(),
                FullName = validationResult.User.FirstName.ToUpper() + " " + validationResult.User.LastName.ToUpper(),
                ExpirationDate = new DateTime(currentTime.Year, currentTime.Month, 1).AddYears(2).AddMonths(1).AddDays(-1),
                Cvv = int.Parse(GenerateNumbers(3)),
                Pin = int.Parse(GenerateNumbers(4)),
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

        private string GenerateNumbers(int length)
        {
            if (length == 0)
            {
                throw new ArgumentException("length parameter is out of range");
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(new Random().Next(0, 9));
            }
            return sb.ToString();
        }

        private async Task<ValidatedCardData> CreateCardValidation(CreateCardRequest createCardRequest)
        {

            if (createCardRequest.UserId <= 0)
            {
                throw new UserNotFoundException("Invalid User ID.");
            }

            if (createCardRequest.AccountId <= 0)
            {
                throw new BankAccountNotFoundException("Invalid Account ID.");
            }

            var UserInfo = await _cardRepository.GetUserFullNameById(createCardRequest.UserId)
                ?? throw new UserNotFoundException("User not found");
            var bankAccount = await _createBankAccountsRepository.GetAccountByIdAsync(createCardRequest.AccountId)
                ?? throw new BankAccountNotFoundException("BankAcount not found.");
            return new() { User = UserInfo, BankAccount = bankAccount };
        }

        public class ValidatedCardData
        {
            public UserResponse User { get; set; }
            public BankAccount BankAccount { get; set; }
        }
    }
}
