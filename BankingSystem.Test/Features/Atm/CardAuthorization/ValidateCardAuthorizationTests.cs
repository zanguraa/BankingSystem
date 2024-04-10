using BankingSystem.Core.Features.Atm.CardAuthorizations;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Test.Factory;
using FakeItEasy;
using BankingSystem.Core.Shared;

namespace BankingSystem.Test.Features.Atm.CardAuthorizations;

[TestFixture]
public class ValidateCardAuthorizationTests
{
	private ICardAuthorizationService _service;
	private ICardAuthorizationRepository _repository;
	private IJwtTokenGenerator _jwtTokenGenerator;
	private ICryptoService _cryptoService;
	private ISeqLogger _logger;

	[SetUp]
	public void SetUp()
	{
		_repository = A.Fake<ICardAuthorizationRepository>();
		_jwtTokenGenerator = A.Fake<IJwtTokenGenerator>();
		_cryptoService = A.Fake<ICryptoService>();
		_logger = A.Fake<ISeqLogger>();
		_service = new CardAuthorizationService(_repository, _jwtTokenGenerator, _cryptoService, _logger);
	}

	[Test]
	public async Task AuthorizeCardAsync_WithNullRequest_ThrowsArgumentNullException()
	{
		Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.AuthorizeCardAsync(null));
	}

	[TestCase("")]
	[TestCase(null)]
	public async Task AuthorizeCardAsync_WithInvalidCardNumber_ThrowsInvalidCardException(string cardNumber)
	{
		var request = ModelFactory.GetCardAuthorizationRequest(r => r.CardNumber = cardNumber);
		Assert.ThrowsAsync<InvalidCardException>(async () => await _service.AuthorizeCardAsync(request));
	}

	[Test]
	public async Task AuthorizeCardAsync_WithInvalidPin_ThrowsInvalidCardException()
	{
		var request = ModelFactory.GetCardAuthorizationRequest(r => r.Pin = "-1");
		Assert.ThrowsAsync<InvalidCardException>(async () => await _service.AuthorizeCardAsync(request));
	}

	[Test]
	public async Task AuthorizeCardAsync_WithInvalidCardNumberLength_ThrowsInvalidCardException()
	{
		var request = ModelFactory.GetCardAuthorizationRequest(r => r.CardNumber = "12345");
		Assert.ThrowsAsync<InvalidCardException>(async () => await _service.AuthorizeCardAsync(request));
	}

	[Test]
	public async Task AuthorizeCardAsync_WithNonNumericCardNumber_ThrowsInvalidCardException()
	{
		var request = ModelFactory.GetCardAuthorizationRequest(r => r.CardNumber = new string('a', 16));
		Assert.ThrowsAsync<InvalidCardException>(async () => await _service.AuthorizeCardAsync(request));
	}
}