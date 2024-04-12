using BankingSystem.Core.Features.Transactions.Shared;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Test.Factory;
using FakeItEasy;

namespace BankingSystem.Test.Features.Transactions.Shared
{
    [TestFixture]
    public class ValidateCreateTransactionRequestTests
    {
        private ITransactionServiceValidator _transactionServiceValidator;
        private ICreateTransactionRepository _fakeCreateTransactionRepository;

        [SetUp]
        public void Setup()
        {
            _fakeCreateTransactionRepository = A.Fake<ICreateTransactionRepository>();
            _transactionServiceValidator = new CreateTransactionServiceValidator(_fakeCreateTransactionRepository);
        }

        [Test]
        public void When_RequestIsNull_ShouldThrow_ArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _transactionServiceValidator.ValidateCreateTransactionRequestAsync(null));
        }

        [Test]
        public void When_UserIdIsEmpty_ShouldThrow_UserValidationException()
        {
            var request = ModelFactory.GetCreateTransactionRequest(r => r.UserId = "");

            Assert.ThrowsAsync<UserValidationException>(() =>
                _transactionServiceValidator.ValidateCreateTransactionRequestAsync(request));
        }

        [Test]
        public void When_FromAccountIdIsInvalid_ShouldThrow_InvalidTransactionValidation()
        {
            var request = ModelFactory.GetCreateTransactionRequest(r => r.FromAccountId = -1);

            Assert.ThrowsAsync<InvalidTransactionException>(() =>
                _transactionServiceValidator.ValidateCreateTransactionRequestAsync(request));
        }

        [Test]
        public void When_ToAccountIdIsInvalid_ShouldThrow_InvalidTransactionValidation()
        {
            var request = ModelFactory.GetCreateTransactionRequest(r => r.ToAccountId = -1);

            Assert.ThrowsAsync<InvalidTransactionException>(() =>
                _transactionServiceValidator.ValidateCreateTransactionRequestAsync(request));
        }

        [Test]
        public void When_AmountIsInvalid_ShouldThrow_InvalidTransactionValidation()
        {
            var request = ModelFactory.GetCreateTransactionRequest(r => r.Amount = -100);

            Assert.ThrowsAsync<InvalidTransactionException>(() =>
                _transactionServiceValidator.ValidateCreateTransactionRequestAsync(request));
        }

		[Test]
		public async Task When_CurrenciesAreInvalid_ShouldThrow_InvalidTransactionException()
		{
			var request = ModelFactory.GetCreateTransactionRequest(r =>
			{
				r.Currency = "INVALID";
				r.ToCurrency = "ALSO_INVALID";
			});

			// Assert that an InvalidTransactionException is thrown when currencies are invalid
			var ex = Assert.ThrowsAsync<InvalidTransactionException>(async () =>
				await _transactionServiceValidator.ValidateCreateTransactionRequestAsync(request));

			Assert.That(ex.Message, Is.EqualTo("One or both currency codes are invalid."));
		}


		[Test]
        public void When_FromAccountIdEqualsToAccountId_ShouldThrow_InvalidAccountException()
        {
            var request = ModelFactory.GetCreateTransactionRequest(r =>
            {
                r.FromAccountId = 1;
                r.ToAccountId = 1;
            });

            Assert.ThrowsAsync<InvalidAccountException>(() =>
                _transactionServiceValidator.ValidateCreateTransactionRequestAsync(request));
        }
    }
}