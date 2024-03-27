using BankingSystem.Core.Features.Atm.CardAuthorizations.Requests;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Test.Factory;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using FakeItEasy;

namespace BankingSystem.Test.Transactions
{
	[TestFixture]
	public class ValidateCardAuthorizationTests
	{
		private ICardAuthorizationService _validator;
		private ICardAuthorizationRepository _fakeCardAuthorizationRepository;

		[SetUp]
		public void Setup()
		{
			_fakeCardAuthorizationRepository = A.Fake<ICardAuthorizationRepository>();
			_validator = new CardAuthorizationService(_fakeCardAuthorizationRepository);
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
