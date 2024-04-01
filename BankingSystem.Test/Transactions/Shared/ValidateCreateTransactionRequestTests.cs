using NUnit.Framework;
using BankingSystem.Core.Features.Transactions.Shared;
using BankingSystem.Core.Features.Transactions.Shared.Models.Requests;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Test.Factory;
using FakeItEasy;
using System;
using System.Threading.Tasks;

namespace BankingSystem.Test.Features.Transactions
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
				_transactionServiceValidator.ValidateCreateTransactionRequest(null));
		}

		[Test]
		public void When_UserIdIsEmpty_ShouldThrow_UserValidationException()
		{
			var request = ModelFactory.GetCreateTransactionRequest(r => r.UserId = "");

			Assert.ThrowsAsync<UserValidationException>(() =>
				_transactionServiceValidator.ValidateCreateTransactionRequest(request));
		}

		[Test]
		public void When_FromAccountIdIsInvalid_ShouldThrow_InvalidTransactionValidation()
		{
			var request = ModelFactory.GetCreateTransactionRequest(r => r.FromAccountId = -1);

			Assert.ThrowsAsync<InvalidTransactionValidation>(() =>
				_transactionServiceValidator.ValidateCreateTransactionRequest(request));
		}

		[Test]
		public void When_ToAccountIdIsInvalid_ShouldThrow_InvalidTransactionValidation()
		{
			var request = ModelFactory.GetCreateTransactionRequest(r => r.ToAccountId = -1);

			Assert.ThrowsAsync<InvalidTransactionValidation>(() =>
				_transactionServiceValidator.ValidateCreateTransactionRequest(request));
		}

		[Test]
		public void When_AmountIsInvalid_ShouldThrow_InvalidTransactionValidation()
		{
			var request = ModelFactory.GetCreateTransactionRequest(r => r.Amount = -100);

			Assert.ThrowsAsync<InvalidTransactionValidation>(() =>
				_transactionServiceValidator.ValidateCreateTransactionRequest(request));
		}

		[Test]
		public void When_CurrenciesAreInvalid_ShouldThrow_ArgumentException()
		{
			var request = ModelFactory.GetCreateTransactionRequest(r =>
			{
				r.Currency = "INVALID";
				r.ToCurrency = "ALSO_INVALID";
			});

			Assert.ThrowsAsync<ArgumentException>(() =>
				_transactionServiceValidator.ValidateCreateTransactionRequest(request));
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
				_transactionServiceValidator.ValidateCreateTransactionRequest(request));
		} 
	} 
}