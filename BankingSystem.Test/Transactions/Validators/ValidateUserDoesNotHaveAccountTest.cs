﻿using NUnit.Framework;
using BankingSystem.Core.Features.BankAccounts.CreateAccount;
using BankingSystem.Core.Features.Users;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Test.Factory;
using FakeItEasy;
using System.Threading.Tasks;
using BankingSystem.Core.Features.BankAccounts.Requests;

namespace BankingSystem.Test.Features.BankAccounts.CreateAccount;

[TestFixture]
public class ValidateUserDoesNotHaveAccountTests
{
	private ICreateBankAccountsService _createBankAccountsService;
	private ICreateBankAccountsRepository _fakeCreateBankAccountsRepository;
	private IUserRepository _fakeUserRepository;
	private ISeqLogger _fakeSeqLogger;

	[SetUp]
	public void Setup()
	{
		_fakeCreateBankAccountsRepository = A.Fake<ICreateBankAccountsRepository>();
		_fakeUserRepository = A.Fake<IUserRepository>();
		_fakeSeqLogger = A.Fake<ISeqLogger>();
		_createBankAccountsService = new CreateBankAccountsService(_fakeCreateBankAccountsRepository, _fakeUserRepository, _fakeSeqLogger);
	}

	[Test]
	public void When_UserIdDoesNotExist_ShouldThrow_UserNotFoundException()
	{
		int userId = 999; // Non-existing user
		A.CallTo(() => _fakeUserRepository.UserExistsAsync(userId)).Returns(false);

		var ex = Assert.ThrowsAsync<UserNotFoundException>(async () => await _createBankAccountsService.CreateBankAccount(new CreateBankAccountRequest { UserId = userId }));
		Assert.That(ex.Message, Is.EqualTo($"user ID {userId} is not exists."));
	}

	[Test]
	public void When_UserIdIsZero_ShouldThrow_UserNotFoundException()
	{
		int userId = 0; // Invalid user ID
		A.CallTo(() => _fakeUserRepository.UserExistsAsync(userId)).Returns(false);

		var ex = Assert.ThrowsAsync<UserNotFoundException>(async () => await _createBankAccountsService.CreateBankAccount(new CreateBankAccountRequest { UserId = userId }));
		Assert.That(ex.Message, Is.EqualTo($"user ID {userId} is not valid."));
	}
}
