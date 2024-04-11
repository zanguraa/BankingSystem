using BankingSystem.Core.Features.BankAccounts.AddFunds;
using BankingSystem.Core.Features.BankAccounts.AddFunds.Models.Requests;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Test.Factory;
using FakeItEasy;

namespace BankingSystem.Test.Features.Transactions.Shared
{
    [TestFixture]
    public class ValidateAddFundsRequestTests
    {
        private IAddFundsService _addFundsService;
        private IAddFundsRepository _fakeAddFundsRepository;

        [SetUp]
        public void Setup()
        {
            _fakeAddFundsRepository = A.Fake<IAddFundsRepository>();
            _addFundsService = new AddFundsService(_fakeAddFundsRepository);
        }

		[Test]
		public void When_AddFundsRequestIsNull_ShouldThrow_ArgumentNullException()
		{
			AddFundsRequest request = null;

            var exception = Assert.ThrowsAsync<ArgumentNullException>(() => _addFundsService.AddFundsAsync(request));
        }


		[TestCase(-100)]
        [TestCase(0)]
        public async Task When_AmountIsLessThanOrEqualToZero_ShouldThrow_InvalidAddFundsValidationException(decimal amount)
        {
            var request = ModelFactory.GetAddFundsRequest(r => r.Amount = amount);

            Assert.ThrowsAsync<InvalidAddFundsValidationException>(() => _addFundsService.AddFundsAsync(request));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public async Task When_BankAccountIdIsLessThanOrEqualToZero_ShouldThrow_InvalidAddFundsValidationException(int bankAccountId)
        {
            var request = ModelFactory.GetAddFundsRequest(r => r.BankAccountId = bankAccountId);

            Assert.ThrowsAsync<InvalidAddFundsValidationException>(() => _addFundsService.AddFundsAsync(request));
        }
    }
}
