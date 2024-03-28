﻿using BankingSystem.Core.Features.Atm.ChangePin;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Test.Factory;
using NUnit.Framework;
using System.Threading.Tasks;
using FakeItEasy;
using BankingSystem.Core.Shared;

namespace BankingSystem.Test.Atm;

[TestFixture]
public class ValidateChangePinAsyncTests
{
	private IChangePinService _changePinService;
	private IChangePinRepository _fakeChangePinRepository;
	private ISeqLogger _fakeLogger;

	[SetUp]
	public void Setup()
	{
		_fakeChangePinRepository = A.Fake<IChangePinRepository>();
		_fakeLogger = A.Fake<ISeqLogger>();
		_changePinService = new ChangePinService(_fakeChangePinRepository, _fakeLogger);
	}

	[Test]
	public async Task ValidateChangePinAsync_InvalidCardNumberFormat_ThrowsInvalidCardException()
	{
		var request = ModelFactory.GetChangePinRequest(r =>
		{
			r.CardNumber = "123456789012345"; // Invalid length
		});

		Assert.ThrowsAsync<InvalidCardException>(
			async () => await _changePinService.ChangePinAsync(request));
	}

	[Test]
	public async Task ValidateChangePinAsync_InvalidCurrentPinFormat_ThrowsInvalidCardException()
	{
		var request = ModelFactory.GetChangePinRequest(r =>
		{
			r.CurrentPin = 123; // Assuming invalid length
		});

		Assert.ThrowsAsync<InvalidCardException>(
			async () => await _changePinService.ChangePinAsync(request));
	}

	[Test]
	public async Task ValidateChangePinAsync_SameNewAndCurrentPin_ThrowsInvalidCardException()
	{
		var request = ModelFactory.GetChangePinRequest(r =>
		{
			r.NewPin = r.CurrentPin; // New PIN is the same as the current PIN
		});

		Assert.ThrowsAsync<InvalidCardException>(
			async () => await _changePinService.ChangePinAsync(request));
	}

	// Add more tests here to cover all scenarios based on your method's logic
}