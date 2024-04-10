using BankingSystem.Core.Features.BankAccounts.AddFunds;
using BankingSystem.Core.Features.BankAccounts.AddFunds.Models.Requests;
using BankingSystem.Core.Features.BankAccounts.CreateAccount;
using BankingSystem.Core.Features.BankAccounts.CreateAccount.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BankAccountController : ControllerBase
{
    private readonly IAddFundsService _addFundsService;
    private readonly ICreateBankAccountsService _createBankAccountsService;

    public BankAccountController(IAddFundsService addFundsService, ICreateBankAccountsService createBankAccountsService)
    {
        _addFundsService = addFundsService;
        _createBankAccountsService = createBankAccountsService;
    }

    [HttpPost("create")]
    [Authorize("OperatorPolicy", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> CreateBankAccount(CreateBankAccountRequest createBankAccountRequest)
    {
        var result = await _createBankAccountsService.CreateBankAccountAsync(createBankAccountRequest);

        return Ok(result);
    }

    [HttpPost("addfunds")]
    [Authorize("OperatorPolicy", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> AddFunds(AddFundsRequest addFundsRequest)
    {
        await _addFundsService.AddFundsAsync(addFundsRequest);
        return Ok();
    }
}

