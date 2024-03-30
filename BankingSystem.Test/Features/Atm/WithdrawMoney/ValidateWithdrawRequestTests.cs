using BankingSystem.Core.Features.Atm.WithdrawMoney;
using BankingSystem.Core.Features.Atm.WithdrawMoney.Models.Requests;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Core.Shared.Services.Currency;
using BankingSystem.Test.Factory;
using FakeItEasy;
using NUnit.Framework;
using System.Threading.Tasks;

namespace BankingSystem.Test.Features.Atm.WithdrawMoney
{
	[TestFixture]
	public class WithdrawMoneyRequestTests
	{
		private IWithdrawMoneyService _withdrawMoneyService;
		private IWithdrawMoneyRepository _withdrawMoneyRepository;
		private ICurrencyConversionService _currencyConversionService;
		private ISeqLogger _seqLogger;

		[SetUp]
		public void Setup()
		{
			_withdrawMoneyRepository = A.Fake<IWithdrawMoneyRepository>();
			_currencyConversionService = A.Fake<ICurrencyConversionService>();
			_seqLogger = A.Fake<ISeqLogger>();
			_withdrawMoneyService = new WithdrawMoneyService(_withdrawMoneyRepository, _currencyConversionService, _seqLogger);
		}

		[Test]
		public async Task ValidateWithdrawRequest_InvalidAmount_ThrowsException()
		{
			var request = ModelFactory.GetWithdrawMoneyRequest(r =>
			{
				r.Amount = 3;
			});
			var cardNumber = "1234567890123456";

			Assert.ThrowsAsync<InvalidAtmAmountException>(() => _withdrawMoneyService.WithdrawAsync(request, cardNumber));
		}

		[Test]
		public async Task ValidateWithdrawRequest_AmountExceedsLimit_ThrowsException()
		{
			var request = ModelFactory.GetWithdrawMoneyRequest(r =>
			{
				r.Amount = 15000; // Exceeds the daily withdrawal limit
			});
			var cardNumber = "1234567890123456"; // Simulating card number

			Assert.ThrowsAsync<InvalidAtmAmountException>(() => _withdrawMoneyService.WithdrawAsync(request, cardNumber));
		}

		[Test]
		public async Task ValidateWithdrawRequest_UnsupportedCurrency_ThrowsException()
		{
			var request = ModelFactory.GetWithdrawMoneyRequest(r =>
			{
				r.Currency = "XYZ"; // Unsupported currency
			});
			var cardNumber = "1234567890123456"; // Simulating card number

			Assert.ThrowsAsync<UnsupportedCurrencyException>(() => _withdrawMoneyService.WithdrawAsync(request, cardNumber));
		}
	}
}
