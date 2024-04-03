using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Test.Factory;
using FakeItEasy;
using BankingSystem.Core.Features.Users.CreateUser;
using BankingSystem.Core.Features.Users.CreateUser.Models.Requests;

namespace BankingSystem.Test.Features.Users
{
    [TestFixture]
	public class ValidateRegisterRequestTests
	{
		private ICreateUserService _createUserService;
		private ICreateUserRepository _fakeCreateUserRepository;

		[SetUp]
		public void Setup()
		{
			_fakeCreateUserRepository = A.Fake<ICreateUserRepository>();
			_createUserService = new CreateUserService(null, _fakeCreateUserRepository);
		}

		[Test]
		public void When_RegisterRequestIsNull_ShouldThrow_ArgumentNullException()
		{
			RegisterUserRequest request = null;

			var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _createUserService.RegisterUser(request));
			Assert.That(ex.ParamName, Is.EqualTo("registerRequest"));
		}

		[Test]
		public void When_EmailIsEmpty_ShouldThrow_UserValidationException()
		{
			var request = ModelFactory.GetRegisterUserRequest(r => r.Email = "");

			var ex = Assert.ThrowsAsync<UserValidationException>(async () => await _createUserService.RegisterUser(request));
			Assert.That(ex.Message, Is.EqualTo("Invalid email address."));
		}

		[Test]
		public void When_EmailIsNull_ShouldThrow_UserValidationException()
		{
			var request = ModelFactory.GetRegisterUserRequest(r => r.Email = null);

			var ex = Assert.ThrowsAsync<UserValidationException>(async () => await _createUserService.RegisterUser(request));
			Assert.That(ex.Message, Is.EqualTo("Invalid email address."));
		}

		[Test]
		public void When_EmailIsInvalid_ShouldThrow_UserValidationException()
		{
			var request = ModelFactory.GetRegisterUserRequest(r => r.Email = "invalidemail");

			var ex = Assert.ThrowsAsync<UserValidationException>(async () => await _createUserService.RegisterUser(request));
			Assert.That(ex.Message, Is.EqualTo("Invalid email address."));
		}

		[Test]
		public void When_BirthdayDateIsMinValue_ShouldThrow_UserValidationException()
		{
			var request = ModelFactory.GetRegisterUserRequest(r => r.BirthdayDate = DateTime.MinValue);

			var ex = Assert.ThrowsAsync<UserValidationException>(async () => await _createUserService.RegisterUser(request));
			Assert.That(ex.Message, Is.EqualTo("Invalid birthday date."));
		}

		[Test]
		public void When_BirthdayDateIsMaxValue_ShouldThrow_UserValidationException()
		{
			var request = ModelFactory.GetRegisterUserRequest(r => r.BirthdayDate = DateTime.MaxValue);

			var ex = Assert.ThrowsAsync<UserValidationException>(async () => await _createUserService.RegisterUser(request));
			Assert.That(ex.Message, Is.EqualTo("Invalid birthday date."));
		}
		[Test]
		public void When_FirstNameIsEmpty_ShouldThrow_UserValidationException()
		{
			var request = ModelFactory.GetRegisterUserRequest(r => r.FirstName = "");

			var ex = Assert.ThrowsAsync<UserValidationException>(async () => await _createUserService.RegisterUser(request));
			Assert.That(ex.Message, Is.EqualTo("First name cannot be empty."));
		}

		[Test]
		public void When_FirstNameIsNull_ShouldThrow_UserValidationException()
		{
			var request = ModelFactory.GetRegisterUserRequest(r => r.FirstName = null);

			var ex = Assert.ThrowsAsync<UserValidationException>(async () => await _createUserService.RegisterUser(request));
			Assert.That(ex.Message, Is.EqualTo("First name cannot be empty."));
		}

		[Test]
		public void When_LastNameIsEmpty_ShouldThrow_UserValidationException()
		{
			var request = ModelFactory.GetRegisterUserRequest(r => r.LastName = "");

			var ex = Assert.ThrowsAsync<UserValidationException>(async () => await _createUserService.RegisterUser(request));
			Assert.That(ex.Message, Is.EqualTo("Last name cannot be empty."));
		}

		[Test]
		public void When_LastNameIsNull_ShouldThrow_UserValidationException()
		{
			var request = ModelFactory.GetRegisterUserRequest(r => r.LastName = null);

			var ex = Assert.ThrowsAsync<UserValidationException>(async () => await _createUserService.RegisterUser(request));
			Assert.That(ex.Message, Is.EqualTo("Last name cannot be empty."));
		}

		[Test]
		public void When_PersonalIdIsEmpty_ShouldThrow_UserValidationException()
		{
			var request = ModelFactory.GetRegisterUserRequest(r => r.PersonalId = "");

			var ex = Assert.ThrowsAsync<UserValidationException>(async () => await _createUserService.RegisterUser(request));
			Assert.That(ex.Message, Is.EqualTo("Personal ID must be exactly 11 characters long."));
		}

		[Test]
		public void When_PersonalIdIsNull_ShouldThrow_UserValidationException()
		{
			var request = ModelFactory.GetRegisterUserRequest(r => r.PersonalId = null);

			var ex = Assert.ThrowsAsync<UserValidationException>(async () => await _createUserService.RegisterUser(request));
			Assert.That(ex.Message, Is.EqualTo("Personal ID must be exactly 11 characters long."));
		}

	}
}
