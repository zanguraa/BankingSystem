using BankingSystem.Core.Features.BankAccounts.CreateAccount;
using BankingSystem.Core.Features.Cards.CreateCard;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Core.Shared.Services;
using BankingSystem.Test.Factory;
using FakeItEasy;

namespace BankingSystem.Test.Features.Cards;

[TestFixture]
public class CreateCardValidationTests
{
	private ICardService _cardService;
	private ICardRepository _cardRepository;
	private ICreateBankAccountsRepository _createBankAccountsRepository;
	private ISeqLogger _seqLogger;
	private ICryptoService _cryptoService;

	[SetUp]
	public void Setup()
	{
		_cardRepository = A.Fake<ICardRepository>();
		_createBankAccountsRepository = A.Fake<ICreateBankAccountsRepository>();
		_seqLogger = A.Fake<ISeqLogger>();
		_cryptoService = A.Fake<ICryptoService>();
		_cardService = new CreateCardService(_cardRepository, _createBankAccountsRepository,_seqLogger,_cryptoService);
	}

	[Test]
	public void When_CreateCardRequestWithInvalidUserId_ShouldThrow_UserNotFoundException()
	{
		var request = ModelFactory.GetCreateCardRequest(r => r.UserId = -1);

		Assert.ThrowsAsync<UserNotFoundException>(() => _cardService.CreateCardAsync(request));
	}

	[Test]
	public void When_CreateCardRequestWithInvalidAccountId_ShouldThrow_BankAccountNotFoundException()
	{
		var request = ModelFactory.GetCreateCardRequest(r => r.AccountId = -1);

		Assert.ThrowsAsync<BankAccountNotFoundException>(() => _cardService.CreateCardAsync(request));
	}
}