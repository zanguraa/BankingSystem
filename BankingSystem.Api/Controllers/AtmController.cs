using BankingSystem.Core.Features.Atm.CardAuthorizations.Models.Requests;
using BankingSystem.Core.Features.Atm.ChangePin;
using BankingSystem.Core.Features.Atm.ChangePin.Models.Requests;
using BankingSystem.Core.Features.Atm.ViewBalance;
using BankingSystem.Core.Features.Atm.WithdrawMoney;
using BankingSystem.Core.Features.Atm.WithdrawMoney.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Api.Controllers;

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
        var result = await _cardAuthorizationService.AuthorizeCardAsync(request);

        return Ok(result);
    }


    [HttpPost("change-pin")]
    [Authorize("AtmPolicy", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> ChangePin([FromBody] ChangePinRequest request)
    {
        var tokenCardNumber = User.Claims.FirstOrDefault(c => c.Type == "CardNumber")?.Value;
        var result = await _changePinService.ChangePinAsync(request, tokenCardNumber);

        return Ok(result);
    }

    [HttpGet("view-balance")]
    [Authorize("AtmPolicy", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> GetBalance()
    {
        var tokenCardNumber = User.Claims.FirstOrDefault(c => c.Type == "CardNumber")?.Value;
        var balanceInfo = await _viewBalanceService.GetBalanceByCardNumberAsync(tokenCardNumber);

        return Ok(balanceInfo);
    }


    [HttpPost("withdraw-money")]
    [Authorize("AtmPolicy", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> Withdraw([FromBody] WithdrawAmountCurrencyRequest requestDto)
    {
        var tokenCardNumber = User.Claims.FirstOrDefault(c => c.Type == "CardNumber")?.Value;

        var responseDto = await _withdrawMoneyService.WithdrawAsync(requestDto, tokenCardNumber);

        return Ok(responseDto);
    }
}