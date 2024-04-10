using BankingSystem.Core.Features.Atm.WithdrawMoney;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Test.Factory;
using FakeItEasy;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Currency;

namespace BankingSystem.Test.Features.Transactions.Shared
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
        public void When_RequestDtoIsNull_ShouldThrow_NullReferenceException()
        {
            var exception = Assert.ThrowsAsync<NullReferenceException>(async () => await _withdrawMoneyService.WithdrawAsync(null, "someCardNumber"));
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