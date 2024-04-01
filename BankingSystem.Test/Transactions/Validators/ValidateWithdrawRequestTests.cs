using NUnit.Framework;
using BankingSystem.Core.Features.Atm.WithdrawMoney;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Core.Features.Atm.WithdrawMoney.Models.Requests;
using BankingSystem.Test.Factory;
using FakeItEasy;
using System.Threading.Tasks;
using BankingSystem.Core.Shared.Services.Currency;
using BankingSystem.Core.Shared.Models;
using BankingSystem.Core.Shared;

namespace BankingSystem.Test.Features.Atm.WithdrawMoney
{
	[TestFixture]
	public class ValidateWithdrawRequestTests
	{
		private IWithdrawMoneyService _withdrawMoneyService;
		private IWithdrawMoneyRepository _fakeWithdrawMoneyRepository;
		private ICurrencyConversionService _fakeCurrencyConversionService;
		private ISeqLogger _fakeSeqLogger;

		[SetUp]
		public void Setup()
		{
			_fakeWithdrawMoneyRepository = A.Fake<IWithdrawMoneyRepository>();
			_fakeCurrencyConversionService = A.Fake<ICurrencyConversionService>();
			_fakeSeqLogger = A.Fake<ISeqLogger>();

			_withdrawMoneyService = new WithdrawMoneyService(_fakeWithdrawMoneyRepository, _fakeCurrencyConversionService, _fakeSeqLogger);
		}

		[Test]
		public async Task When_RequestDtoIsNull_ShouldThrow_ArgumentNullException()
		{
			// Act & Assert
			var exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await _withdrawMoneyService.WithdrawAsync(null, "someCardNumber"));

			// Assert
			Assert.That(exception.ParamName, Is.EqualTo("requestDto"));
		}

		[TestCase(-1)] 
		[TestCase(0)] 
		public async Task When_AmountIsLessThanOrEqualToZero_ShouldThrow_InvalidAtmAmountException(int amount)
		{
			var request = ModelFactory.GetWithdrawAmountCurrencyRequest(r => r.Amount = amount);
			Assert.ThrowsAsync<InvalidAtmAmountException>(() => _withdrawMoneyService.WithdrawAsync(request, "1234567890123456"));
		}

		[TestCase("INVALID")]
		public async Task When_CurrencyIsUnsupported_ShouldThrow_UnsupportedCurrencyException(string currency)
		{
			var request = ModelFactory.GetWithdrawAmountCurrencyRequest(r => r.Currency = currency);

			Assert.ThrowsAsync<UnsupportedCurrencyException>(() => _withdrawMoneyService.WithdrawAsync(request, "1234567890123456"));
		}
	}
}