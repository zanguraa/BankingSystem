using Azure.Core;
using BankingSystem.Core.Features.Transactions.CreateTransactions;
using BankingSystem.Core.Features.Transactions.TransactionServices;
using BankingSystem.Core.Features.Transactions.TransactionsRepositories;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Test.Factory;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			var request = ModelFactory.GetCreateTransactionRequest(
				r=>r.UserId=userId);

			Assert.ThrowsAsync<UserValidationException>(
			   async () => await _validator.ValidateCreateTransactionRequest(request));
		}
		[Test]
		public void ShouldThrowInvalidTransactionValidationIfRequestFromAccountIdIsLessOrEqualZero()
		{
			var request = ModelFactory.GetCreateTransactionRequest(
				r => r.FromAccountId = -23);

			Assert.ThrowsAsync<InvalidTransactionValidation>(
			   async () => await _validator.ValidateCreateTransactionRequest(request));
		}
		[Test]
		public void ShouldThrowInvalidTransactionValidationIfRequestToAccountIdIsLessOrEqualZero() 
		{
			var request = ModelFactory.GetCreateTransactionRequest(
				r => r.ToAccountId = -12); Assert.ThrowsAsync<InvalidTransactionValidation>(
				async () => await _validator.ValidateCreateTransactionRequest(request));
		}
	}
}
