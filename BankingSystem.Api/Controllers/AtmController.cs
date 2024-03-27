using BankingSystem.Core.Features.Atm.CardAuthorizations.Requests;
using BankingSystem.Core.Features.Atm.ChangePin;
using BankingSystem.Core.Features.Atm.ChangePin.Requests;
using BankingSystem.Core.Features.Atm.ViewBalance;
using BankingSystem.Core.Features.Atm.WithdrawMoney.Requests;
using BankingSystem.Core.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AtmController : ControllerBase
    {
        private readonly ICardAuthorizationService _cardAuthorizationService;
        private readonly IChangePinService _changePinService;
        private readonly IViewBalanceService _viewBalanceService;
        private readonly IWithdrawMoneyService _withdrawMoneyService;
        private readonly JwtTokenGenerator _jwtTokenGenerator;


        public AtmController(ICardAuthorizationService cardAuthorizationService, IChangePinService changePinService, IViewBalanceService viewBalanceService, IWithdrawMoneyService withdrawMoneyService, JwtTokenGenerator jwtTokenGenerator)
        {
            _cardAuthorizationService = cardAuthorizationService;
            _changePinService = changePinService;
            _viewBalanceService = viewBalanceService;
            _withdrawMoneyService = withdrawMoneyService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost("card-authorize")]
        public async Task<IActionResult> Authorize([FromBody] CardAuthorizationRequest request)
        {
            var result = await _cardAuthorizationService.AuthorizeCardAsync(request);

            if (!result) { return BadRequest(); }
            var token = _jwtTokenGenerator.Generate(request.CardNumber, "atm");

            return Ok(token);
        }


        [HttpPost("change-pin")]
        [Authorize("AtmPolicy", AuthenticationSchemes = "Bearer")]

        public async Task<IActionResult> ChangePin([FromBody] ChangePinRequest request)
        {
            var result = await _changePinService.ChangePinAsync(request);
            if (!result) { return BadRequest(); }
            return Ok(new ChangePinResponse { Success = true, Message = "PIN changed successfully." });
        }

        [HttpGet("view-balance")]
        [Authorize("AtmPolicy", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetBalance()
        {
            var cardNumberClaim = User.Claims.FirstOrDefault(c => c.Type == "CardNumber")?.Value;

            if (string.IsNullOrEmpty(cardNumberClaim))
            {
                return BadRequest("Invalid token: Card number claim is missing.");
            }

            var balanceInfo = await _viewBalanceService.GetBalanceByCardNumberAsync(cardNumberClaim);
            if (balanceInfo == null)
            {
                return NotFound("No balance information found for the provided card number.");
            }

            return Ok(balanceInfo);
        }


        [HttpPost("withdraw-money")]
        [Authorize("AtmPolicy", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawAmountCurrency requestDto)
        {
            var tokenCardNumber = User.Claims.FirstOrDefault(c => c.Type == "CardNumber")?.Value;

            if (tokenCardNumber == null)
            {
                return Unauthorized("The card number does not match the authorized user.");
            }

            var requestWithCardNumber = new WithdrawRequestWithCardNumber()
            {
                Amount = requestDto.Amount,
                CardNumber = tokenCardNumber,
                Currency = requestDto.Currency
            };

            var responseDto = await _withdrawMoneyService.WithdrawAsync(requestWithCardNumber);
            if (responseDto.IsSuccessful)
            {
                return Ok(responseDto);
            }
            else
            {
                return BadRequest(responseDto);
            }
        }
    }
}