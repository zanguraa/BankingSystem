using BankingSystem.Core.Features.Atm.ViewBalance;
using BankingSystem.Core.Shared.Exceptions;
using FakeItEasy;

namespace BankingSystem.Test.Features.Atm.ViewBalance
{
    [TestFixture]
	public class GetbalanceByCardNumberValidatorTests
	{
		private IViewBalanceService _viewBalanceService;
		private IViewBalanceRepository _fakeViewBalanceRepository;

		[SetUp]
		public void Setup()
		{
			_fakeViewBalanceRepository = A.Fake<IViewBalanceRepository>();
			_viewBalanceService = new ViewBalanceService(_fakeViewBalanceRepository);
		}

		[Test]
		public void When_CardNumberIsEmpty_ShouldThrow_InvalidCardException()
		{
			string cardNumber = "";

			var ex = Assert.ThrowsAsync<InvalidCardException>(async () => await _viewBalanceService.GetBalanceByCardNumberAsync(cardNumber));
			Assert.That(ex.Message, Is.EqualTo("Card Number is not Found."));
		}

		[Test]
		public void When_CardNumberIsNull_ShouldThrow_InvalidCardException()
		{
			string cardNumber = null;

			var ex = Assert.ThrowsAsync<InvalidCardException>(async () => await _viewBalanceService.GetBalanceByCardNumberAsync(cardNumber));
			Assert.That(ex.Message, Is.EqualTo("Card Number is not Found."));
		}
	}
}
