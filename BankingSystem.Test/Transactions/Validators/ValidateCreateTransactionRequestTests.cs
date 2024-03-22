using BankingSystem.Core.Features.Transactions.CreateTransactions;
using BankingSystem.Core.Features.Transactions.TransactionServices;
using BankingSystem.Core.Features.Transactions.TransactionsRepositories;
using BankingSystem.Core.Shared.Exceptions;
using FakeItEasy;

namespace BankingSystem.Test.Transactions.Validators
{
    public class ValidateCreateTransactionRequestTests
	{
		private ITransactionServiceValidator _validator;
		private ITransactionRepository _fakeTransactionRepository;

		[SetUp]
		public void Setup()
		{
			_fakeTransactionRepository = A.Fake<ITransactionRepository>();
			_validator = new TransactionServiceValidator(_fakeTransactionRepository);
		}

		[Test]
		public void ShouldThrowArgumentNullExceptionIfRequestIsNull()
		{
			CreateTransactionRequest request = null;
			Assert.ThrowsAsync<ArgumentNullException>(
				async () => await _validator.ValidateCreateTransactionRequest(request));
		}

		[TestCase("")]
		[TestCase(null)]
		public void ShouldThrowInvalidTransactionValidationIfRequestUsedIdIsNullOrEmpty(string userId)
		{
			CreateTransactionRequest request = new CreateTransactionRequest() { UserId = userId };
			Assert.ThrowsAsync<InvalidTransactionValidation>(
			   async () => await _validator.ValidateCreateTransactionRequest(request));
		}
		[Test]
		public void ShouldThrowInvalidTransactionValidationIfRequestFromAccountIdIsLessOrEqualZero()
		{
			CreateTransactionRequest request = new CreateTransactionRequest() { FromAccountId = -23 };
			Assert.ThrowsAsync<InvalidTransactionValidation>(
			   async () => await _validator.ValidateCreateTransactionRequest(request));
		}
		[Test]
		public void ShouldThrowInvalidTransactionValidationIfRequestToAccountIdIsLessOrEqualZero() 
		{
			CreateTransactionRequest request = new CreateTransactionRequest() { ToAccountId = -4 };
			Assert.ThrowsAsync<InvalidTransactionValidation>(
				async () => await _validator.ValidateCreateTransactionRequest(request));
		}
	}
}
