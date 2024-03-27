using BankingSystem.Core.Features.Atm.CardAuthorizations.Requests;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Test.Factory;
using NUnit.Framework;

namespace BankingSystem.Test.Transactions.Validators;

[TestFixture]
public class ValidateChangePinAsyncTests
{
	private IChangePinValidator _validator;

	[SetUp]
	public void Setup()
	{
		// Assuming you have a validator that exposes the method now.
		_validator = new ChangePinValidator();
	}

	[Test]
	public void ValidateChangePinAsync_InvalidCardNumberFormat_ThrowsInvalidCardException()
	{
		var request = ModelFactory.GetChangePinRequest(r =>
		{
			r.CardNumber = "123"; // Invalid length
		});

		Assert.Throws<InvalidCardException>(() => _validator.ValidateChangePinAsync(request));
	}

	[Test]
	public void ValidateChangePinAsync_InvalidCurrentPinFormat_ThrowsInvalidCardException()
	{
		var request = ModelFactory.GetChangePinRequest(r =>
		{
			r.CurrentPin = 123; // Invalid length
		});

		Assert.Throws<InvalidCardException>(() => _validator.ValidateChangePinAsync(request));
	}

	[Test]
	public void ValidateChangePinAsync_SameNewAndCurrentPin_ThrowsInvalidCardException()
	{
		var request = ModelFactory.GetChangePinRequest(r =>
		{
			r.NewPin = r.CurrentPin; // New PIN is the same as the current PIN
		});

		Assert.Throws<InvalidCardException>(() => _validator.ValidateChangePinAsync(request));
	}
}