using BankingSystem.Core.Features.BankAccounts.AddFunds;
using BankingSystem.Core.Features.BankAccounts.CreateAccount;
using BankingSystem.Core.Features.BankAccounts.Requests;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> CreateBankAccount(CreateBankAccountRequest createBankAccountRequest)
    {
            if (createBankAccountRequest == null || createBankAccountRequest.UserId == default(int))
            {
                return BadRequest("UserId is required.");
            }

            await _createBankAccountsService.CreateBankAccount(createBankAccountRequest);

            return Ok("Bank Account has created successfully"); 
    }

    [HttpPost("addfunds")]
    public async Task<IActionResult> AddFunds(AddFundsRequest addFundsRequest)
    {
        await _addFundsService.AddFunds(addFundsRequest);
        return Ok();
    }
}

