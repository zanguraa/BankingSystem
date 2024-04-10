using BankingSystem.Core.Features.BankAccounts.CreateAccount;
using BankingSystem.Core.Features.BankAccounts.CreateAccount.Models.Requests;
using BankingSystem.Core.Features.Users.AuthorizeUser;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Exceptions;
using FakeItEasy;

namespace BankingSystem.Test.Features.BankAccounts.CreateAccount
{
    [TestFixture]
	public class ValidateUserDoesNotHaveAccountTests
	{
		private ICreateBankAccountsService _createBankAccountsService;
		private ICreateBankAccountsRepository _createBankAccountsRepository;
		private IAuthorizeUserRepository _authorizeUserRepository;
		private ISeqLogger _seqLogger;

		[SetUp]
		public void Setup()
		{
			_createBankAccountsRepository = A.Fake<ICreateBankAccountsRepository>();
			_authorizeUserRepository = A.Fake<IAuthorizeUserRepository>();
			_seqLogger = A.Fake<ISeqLogger>();
			_createBankAccountsService = new CreateBankAccountsService(_createBankAccountsRepository, _authorizeUserRepository, _seqLogger);
		}

		[Test]
		public async Task When_UserIdDoesNotExist_ShouldThrow_UserNotFoundException()
		{
			int userId = 999; 
			A.CallTo(() => _authorizeUserRepository.UserExistsAsync(userId)).Returns(false);

			Assert.ThrowsAsync<UserNotFoundException>(async () => await _createBankAccountsService.CreateBankAccountAsync(new CreateBankAccountRequest { UserId = userId }));
		}

		[Test]
		public async Task When_UserIdIsZero_ShouldThrow_UserNotFoundException()
		{
			int userId = 0; 
			A.CallTo(() => _authorizeUserRepository.UserExistsAsync(userId)).Returns(true); 

			Assert.ThrowsAsync<UserNotFoundException>(async () => await _createBankAccountsService.CreateBankAccountAsync(new CreateBankAccountRequest { UserId = userId }));
		}
	}
}
