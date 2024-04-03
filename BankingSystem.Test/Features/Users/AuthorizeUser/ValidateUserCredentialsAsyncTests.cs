using BankingSystem.Core.Data.Entities;
using BankingSystem.Core.Features.Users.AuthorizeUser;
using BankingSystem.Core.Features.Users.AuthorizeUser.Requests;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Test.Factory;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;

namespace BankingSystem.Test.Features.Users;

[TestFixture]
public class ValidateUserCredentialsAsyncTests
{
	private IAuthorizeUserService _authorizeUserService;
	private UserManager<UserEntity> _userManagerMock;
	private IJwtTokenGenerator _jwtTokenGeneratorMock;

	[SetUp]
	public void Setup()
	{
		var store = A.Fake<IUserStore<UserEntity>>();
		_userManagerMock = A.Fake<UserManager<UserEntity>>();
		_jwtTokenGeneratorMock = A.Fake<IJwtTokenGenerator>();

		_authorizeUserService = new AuthorizeUserService(_userManagerMock, _jwtTokenGeneratorMock);
	}

	[Test]
	public async Task AuthorizeUser_WithValidCredentials_ShouldNotThrowExceptions()
	{
		var loginRequest = ModelFactory.GetLoginRequest();
		var fakeUser = new UserEntity { Email = loginRequest.Email };

		A.CallTo(() => _userManagerMock.FindByEmailAsync(loginRequest.Email)).Returns(fakeUser);
		A.CallTo(() => _userManagerMock.CheckPasswordAsync(fakeUser, loginRequest.Password)).Returns(true);

		Assert.DoesNotThrowAsync(async () => await _authorizeUserService.AuthorizeUser(loginRequest));
	}

	[Test]
	public async Task AuthorizeUser_WithInvalidEmail_ShouldThrowUserNotFoundException()
	{
		var fakeUserManager = A.Fake<UserManager<UserEntity>>(); 
		var loginRequest = new LoginRequest { Email = "invalid@example.com", Password = "password" };
		var userService = new AuthorizeUserService(fakeUserManager, _jwtTokenGeneratorMock);

		A.CallTo(() => fakeUserManager.FindByEmailAsync(loginRequest.Email)).Returns(Task.FromResult<UserEntity>(null));

		var ex = Assert.ThrowsAsync<UserNotFoundException>(async () => await userService.AuthorizeUser(loginRequest));

		Assert.That(ex, Is.TypeOf<UserNotFoundException>());
	}


	[Test]
	public async Task AuthorizeUser_WithIncorrectPassword_ShouldThrowUserNotFoundException()
	{
		var loginRequest = ModelFactory.GetLoginRequest();
		var fakeUser = new UserEntity { Email = loginRequest.Email };

		A.CallTo(() => _userManagerMock.FindByEmailAsync(loginRequest.Email)).Returns(fakeUser);
		A.CallTo(() => _userManagerMock.CheckPasswordAsync(fakeUser, loginRequest.Password)).Returns(false);

		var ex = Assert.ThrowsAsync<UserNotFoundException>(async () => await _authorizeUserService.AuthorizeUser(loginRequest));
		Assert.That(ex.Message, Is.EqualTo($"Email {loginRequest.Email} or Password is incorrect"));
	}
}