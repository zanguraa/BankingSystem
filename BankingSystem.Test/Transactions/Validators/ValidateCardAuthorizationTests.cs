using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Test.Factory;
using FakeItEasy;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Features.Atm.CardAuthorizations.Models.Requests;

namespace BankingSystem.Test.Transactions
{
    [TestFixture]
	public class ValidateCardAuthorizationTests
	{
		private ICardAuthorizationService _validator;
		private ICardAuthorizationRepository _fakeCardAuthorizationRepository;
		private IJwtTokenGenerator _fakeJwtTokenGenerator;
		private ISeqLogger _fakeSeqLogger;



		[SetUp]
		public void Setup()
		{
			_fakeCardAuthorizationRepository = A.Fake<ICardAuthorizationRepository>();
			_fakeJwtTokenGenerator = A.Fake<IJwtTokenGenerator>();
			_fakeSeqLogger = A.Fake<ISeqLogger>();
			_validator = new CardAuthorizationService(_fakeCardAuthorizationRepository, _fakeJwtTokenGenerator, _fakeSeqLogger);
		}

		[Test]
		public void ValidateCardAuthorization_NullRequest_ThrowsArgumentNullException()
		{

			CardAuthorizationRequest request = null;

			// Note: Using ThrowsAsync because the method under test is async.
			var exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await _validator.AuthorizeCardAsync(request));
			Assert.That(exception.ParamName, Is.EqualTo("request")); // Assuming your method uses nameof(request) for the exception
		}

		[Test]
		public void ValidateCardAuthorization_EmptyCardNumber_ThrowsInvalidCardException()
		{
			var request = ModelFactory.GetCardAuthorizationRequest(r => r.CardNumber = "");

			// Using ThrowsAsync for async method
			Assert.ThrowsAsync<InvalidCardException>(async () => await _validator.AuthorizeCardAsync(request));
		}

		[Test]
		public void ValidateCardAuthorization_InvalidPin_ThrowsInvalidCardException()
		{
			var request = ModelFactory.GetCardAuthorizationRequest(r => r.Pin = -1);

			// Using ThrowsAsync for async method
			Assert.ThrowsAsync<InvalidCardException>(async () => await _validator.AuthorizeCardAsync(request));
		}

		[Test]
		public void ValidateCardAuthorization_InvalidCardNumberLength_ThrowsInvalidCardException()
		{
			// Setup a card number with invalid length
			var request = ModelFactory.GetCardAuthorizationRequest(r => r.CardNumber = "1234567890123"); // Less than 16 digits

			// Using ThrowsAsync for async method
			Assert.ThrowsAsync<InvalidCardException>(async () => await _validator.AuthorizeCardAsync(request));
		}

		[Test]
		public void ValidateCardAuthorization_NonNumericCardNumber_ThrowsInvalidCardException()
		{
			// Setup a card number with non-numeric characters
			var request = ModelFactory.GetCardAuthorizationRequest(r => r.CardNumber = new string('a', 16)); // Non-numeric

			// Using ThrowsAsync for async method
			Assert.ThrowsAsync<InvalidCardException>(async () => await _validator.AuthorizeCardAsync(request));
		}
	}

}
