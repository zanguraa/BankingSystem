using BankingSystem.Core.Features.Atm.ChangePin;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Test.Factory;
using FakeItEasy;
using BankingSystem.Core.Shared.Models;
using BankingSystem.Core.Shared;

namespace BankingSystem.Test.Features.Atm.ChangePin;

[TestFixture]
public class ValidateChangePinAsyncTests
{
	private IChangePinService _changePinService;
	private IChangePinRepository _changePinRepository;
	private ISeqLogger _logger;

	[SetUp]
	public void SetUp()
	{
		_changePinRepository = A.Fake<IChangePinRepository>();
		_logger = A.Fake<ISeqLogger>();
		_changePinService = new ChangePinService(_changePinRepository, _logger);
	}

	[Test]
	public async Task ValidateChangePinAsync_InvalidCardNumberFormat_ThrowsInvalidCardException()
	{
		var request = ModelFactory.GetChangePinRequest(r => r.CardNumber = "1234567890123456"); // Valid length for setup
		A.CallTo(() => _changePinRepository.GetCardByNumberAsync(request.CardNumber)).Returns(Task.FromResult((Card)null)); // Simulate card not found

		Assert.ThrowsAsync<InvalidCardException>(async () => await _changePinService.ChangePinAsync(request, request.CardNumber));
	}

	[Test]
	public async Task ValidateChangePinAsync_InvalidCurrentPinFormat_ThrowsInvalidCardException()
	{
		var request = ModelFactory.GetChangePinRequest(r => r.CurrentPin = 1234); // Valid format for setup
		A.CallTo(() => _changePinRepository.GetCardByNumberAsync(request.CardNumber)).Returns(Task.FromResult(new Card { Pin = 5678 })); // Simulate current PIN mismatch

		Assert.ThrowsAsync<InvalidCardException>(async () => await _changePinService.ChangePinAsync(request, request.CardNumber));
	}

	[Test]
	public async Task ValidateChangePinAsync_SameNewAndCurrentPin_ThrowsInvalidCardException()
	{
		var request = ModelFactory.GetChangePinRequest(r =>
		{
			r.CurrentPin = 1234;
			r.NewPin = 1234; // New PIN is the same as the current PIN
		});
		A.CallTo(() => _changePinRepository.GetCardByNumberAsync(request.CardNumber)).Returns(Task.FromResult(new Card { Pin = request.CurrentPin })); // Simulate valid current PIN

		Assert.ThrowsAsync<InvalidCardException>(async () => await _changePinService.ChangePinAsync(request, request.CardNumber));
	}
}
