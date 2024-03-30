using NUnit.Framework;
using BankingSystem.Core.Features.BankAccounts.AddFunds;
using BankingSystem.Core.Features.BankAccounts.AddFunds.Models.Requests;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Test.Factory;
using FakeItEasy;
using System.Threading.Tasks;

namespace BankingSystem.Test.Features.BankAccounts.AddFunds
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

            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _addFundsService.AddFunds(request));
            Assert.That(ex.ParamName, Is.EqualTo("request"));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void When_AmountIsLessThanOrEqualToZero_ShouldThrow_InvalidAddFundsValidationException(decimal amount)
        {
            var request = ModelFactory.GetAddFundsRequest(r => r.Amount = amount);

            var ex = Assert.ThrowsAsync<InvalidAddFundsValidationException>(async () => await _addFundsService.AddFunds(request));
            Assert.That(ex.Message, Is.EqualTo("The amount must be greater than zero."));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void When_BankAccountIdIsLessThanOrEqualToZero_ShouldThrow_InvalidAddFundsValidationException(int bankAccountId)
        {
            var request = ModelFactory.GetAddFundsRequest(r => r.BankAccountId = bankAccountId);

            var ex = Assert.ThrowsAsync<InvalidAddFundsValidationException>(async () => await _addFundsService.AddFunds(request));
            Assert.That(ex.Message, Is.EqualTo("The Bank Account ID must be a positive number."));
        }
    }
}
