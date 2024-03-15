using BankingSystem.Core.Features.Atm.CardAuthorization;
using BankingSystem.Core.Features.Atm.CardAuthorizations.Requests;
using BankingSystem.Core.Features.Atm.ChangePin;
using BankingSystem.Core.Features.Atm.ChangePin.Requests;
using BankingSystem.Core.Features.Atm.ViewBalance;
using BankingSystem.Core.Features.Atm.WithdrawMoney;
using BankingSystem.Core.Features.Atm.WithdrawMoney.Requests;
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

        public AtmController(ICardAuthorizationService cardAuthorizationService, IChangePinService changePinService, IViewBalanceService viewBalanceService, IWithdrawMoneyService withdrawMoneyService)
        {
            _cardAuthorizationService = cardAuthorizationService;
            _changePinService = changePinService;
            _viewBalanceService = viewBalanceService;
            _withdrawMoneyService = withdrawMoneyService;
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

                var responseDto = new CardAuthorizationResponse
                {
                    IsAuthorized = isAuthorized,
                    Message = isAuthorized ? "Authorization successful." : "Authorization failed."
                };

                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("change-pin")]
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

        [HttpGet("view-balance{userId}")]
        public async Task<IActionResult> GetBalance(string userId)
        {
            try
            {
                var balanceDto = await _viewBalanceService.GetBalanceByUserIdAsync(userId);
                if (balanceDto == null)
                {
                    return NotFound($"No balance information found for user ID: {userId}.");
                }
                return Ok(balanceDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("withdraw-money")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawRequest requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var responseDto = await _withdrawMoneyService.WithdrawAsync(requestDto);
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