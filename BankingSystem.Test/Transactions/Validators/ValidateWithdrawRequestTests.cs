using BankingSystem.Core.Features.Atm.WithdrawMoney;
using BankingSystem.Test.Factory;
using NUnit.Framework;
using System;
using BankingSystem.Core.Data;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Core.Features.Transactions.TransactionServices;
using BankingSystem.Core.Features.Transactions.TransactionsRepositories;
using FakeItEasy;
using BankingSystem.Core.Features.Atm.ViewBalance;
using BankingSystem.Core.Features.Transactions.Currency;
using BankingSystem.Core.Shared;

namespace BankingSystem.Test.Transactions
{
	[TestFixture]
	public class WithdrawMoneyRequestTests
	{
		private IWithdrawMoneyService _validator;
		private IWithdrawMoneyRepository _fakeWithdrawMoneyRepository;
		private ITransactionRepository _fakeTransactionRepository;
		private IViewBalanceRepository _fakeViewBalanceRepository;
		private ICardAuthorizationRepository _fakeCardAuthorizationRepository;
		private ICurrencyConversionService _fakeCurrencyConversionService;
		private ISeqLogger _fakeSeqLogger;


		[SetUp]
		public void Setup()
		{
			_fakeWithdrawMoneyRepository = A.Fake<IWithdrawMoneyRepository>();
			_fakeTransactionRepository = A.Fake<ITransactionRepository>();
			_fakeViewBalanceRepository = A.Fake<IViewBalanceRepository>();
			_fakeCardAuthorizationRepository = A.Fake<ICardAuthorizationRepository>();
			_fakeCurrencyConversionService = A.Fake<ICurrencyConversionService>();
			_fakeSeqLogger= A.Fake<SeqLogger>();
			_validator = new WithdrawMoneyService(_fakeWithdrawMoneyRepository ,
													_fakeCurrencyConversionService,
													_fakeCardAuthorizationRepository,
												   _fakeViewBalanceRepository,
												   _fakeTransactionRepository,
												   _fakeSeqLogger
												);
		}
		
		[Test]
		public void ValidateWithdrawRequest_InvalidAmount_ThrowsException()
		{
			var request = ModelFactory.GetWithdrawMoneyRequest(r =>
			{
				r.Amount = 3; 
			});

			Assert.ThrowsAsync<InvalidAtmAmountException>(async () => await _validator.WithdrawAsync(request));
		}

		[Test]
		public void ValidateWithdrawRequest_AmountExceedsLimit_ThrowsException()
		{
			var request = ModelFactory.GetWithdrawMoneyRequest(r =>
			{
				r.Amount = 15000; 
			});

			Assert.ThrowsAsync<InvalidAtmAmountException>(async () => await _validator.WithdrawAsync(request));
		}

		[Test]
		public void ValidateWithdrawRequest_UnsupportedCurrency_ThrowsException()
		{
			var request = ModelFactory.GetWithdrawMoneyRequest(r =>
			{
				r.Currency = "XYZ"; 
			});

			Assert.ThrowsAsync<UnsupportedCurrencyException>(async () => await _validator.WithdrawAsync(request));
		}
	}
}