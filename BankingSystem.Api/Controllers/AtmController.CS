using BankingSystem.Core.Features.Atm.CardAuthorization;
using BankingSystem.Core.Features.Atm.CardAuthorizations.Requests;
using BankingSystem.Core.Features.Atm.ChangePin;
using BankingSystem.Core.Features.Atm.ChangePin.Requests;
using BankingSystem.Core.Features.Atm.ViewBalance;
using BankingSystem.Core.Features.Atm.WithdrawMoney;
using BankingSystem.Core.Features.Atm.WithdrawMoney.Requests;
using BankingSystem.Core.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var isAuthorized = await _cardAuthorizationService.AuthorizeCardAsync(request);

                if (isAuthorized)
                {
                    var token = _jwtTokenGenerator.GenerateTokenForAtmOperations(request.CardNumber, "CustomRoleOrClaimHere");

                    return Ok(new
                    {
                        IsAuthorized = isAuthorized,
                        Message = "Authorization successful.",
                        Token = token
                    });
                }
                else
                {
                    return BadRequest(new { IsAuthorized = false, Message = "Authorization failed." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpPost("change-pin")]
        [Authorize("CardHolder", AuthenticationSchemes = "Bearer")]

        public async Task<IActionResult> ChangePin([FromBody] ChangePinRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _changePinService.ChangePinAsync(request.CardNumber, request.CurrentPin, request.NewPin);

                if (result)
                {
                    return Ok(new ChangePinResponse { Success = true, Message = "PIN changed successfully." });
                }
                else
                {
                    return BadRequest(new ChangePinResponse { Success = false, Message = "Failed to change PIN. Please check the current PIN and try again." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ChangePinResponse { Success = false, Message = "An internal error occurred. Please try again later." });
            }
        }

        [HttpGet("view-balance")]
        [Authorize("CardHolder", AuthenticationSchemes = "Bearer")]
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
        [Authorize("CardHolder", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawAmountCurrency requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Extract the card number from the JWT token's claims
            var tokenCardNumber = User.Claims.FirstOrDefault(c => c.Type == "CardNumber")?.Value;

            // Check if the card number in the token matches the card number in the request
            if (tokenCardNumber == null)
            {
                return Unauthorized("The card number does not match the authorized user.");
            }

            try
            {
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
            catch (Exception ex)
            {
                return StatusCode(500, "An internal server error has occurred.");
            }
        }

    }
}