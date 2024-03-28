using NUnit.Framework;
using BankingSystem.Core.Features.BankAccounts.AddFunds;
using BankingSystem.Test.Factory;
using System;
using BankingSystem.Core.Shared.Exceptions;
using FakeItEasy;
using BankingSystem.Core.Features.BankAccounts.AddFunds.Models.Requests;
using BankingSystem.Core.Features.Transactions.CreateTransactions;

namespace BankingSystem.Test.Features.BankAccounts.AddFunds
{
    [TestFixture]
	public class ValidateAddFundsRequestTests
	{
		private IAddFundsService _addFundsService;
		private IAddFundsRepository _fakeAddFundsRepository;
		private ITransactionRepository _fakeTransactionRepository;

		[SetUp]
		public void Setup()
		{
			_fakeAddFundsRepository = A.Fake<IAddFundsRepository>();
			_fakeTransactionRepository = A.Fake<ITransactionRepository>();
			_addFundsService = new AddFundsService(_fakeAddFundsRepository, _fakeTransactionRepository);
		}

		[Test]
		public void When_AddFundsRequestIsNull_ShouldThrow_ArgumentNullException()
		{
			AddFundsRequest request = null;

			var ex = Assert.ThrowsAsync<ArgumentNullException>(() => _addFundsService.AddFunds(request));
			Assert.That(ex.ParamName, Is.EqualTo("request"));
			Assert.That(ex.Message, Does.Contain("The request cannot be null."));
		}

		[Test]
		public void When_AmountIsLessThanOrEqualToZero_ShouldThrow_InvalidAddFundsValidationException()
		{
			var request = ModelFactory.GetAddFundsRequest(r => r.Amount = 0);

			Assert.ThrowsAsync<InvalidAddFundsValidationException>(() => _addFundsService.AddFunds(request));
		}
		
		[Test]
		public void When_BankAccountIdIsLessThanOrEqualToZero_ShouldThrow_InvalidAddFundsValidationException()
		{
			var request = ModelFactory.GetAddFundsRequest(r => r.BankAccountId = 0);

			Assert.ThrowsAsync<InvalidAddFundsValidationException>(() => _addFundsService.AddFunds(request));
		}
	}
}
