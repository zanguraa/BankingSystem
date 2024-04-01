using BankingSystem.Core.Features.Transactions.CreateTransactions;
using BankingSystem.Core.Features.Transactions.Shared;
using BankingSystem.Core.Features.Transactions.Shared.Models.Requests;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Test.Factory;
using FakeItEasy;
using NuGet.Frameworks;

namespace BankingSystem.Test.Features.Transactions.Validators
{
    [TestFixture]
    public class ValidateCreateTransactionRequestTests
    {
        private ITransactionServiceValidator _validator;
		private  ICreateTransactionRepository _fakeRepository;

        [SetUp]
        public void Setup() {
            _fakeRepository = A.Fake<ICreateTransactionRepository>();
            _validator = new CreateTransactionServiceValidator(_fakeRepository);
        }

        [Test]
        public void When_RequestIsNull_ShouldThrow_ArgumentNullException() 
        {
            CreateTransactionRequest request = null;  
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _validator.ValidateCreateTransactionRequest(request));
            Assert.That(ex.Message, Is.EqualTo("Transaction request cannot be null."));
		}
	}
}
