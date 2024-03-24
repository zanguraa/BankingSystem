using BankingSystem.Core.Features.BankAccounts.AddFunds;
using BankingSystem.Core.Features.BankAccounts.CreateAccount;
using BankingSystem.Core.Features.BankAccounts.Requests;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BankAccountController : ControllerBase
{
    private readonly IBankAccountService _bankAccountService;
    private readonly IAddFundsService _addFundsService;

    public BankAccountController(IBankAccountService bankAccountService, IAddFundsService addFundsService)
    {
        _bankAccountService = bankAccountService;
        _addFundsService = addFundsService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateBankAccount(CreateBankAccountRequest createBankAccountRequest)
    {
        try
        {
            if (createBankAccountRequest == null || createBankAccountRequest.UserId == default(int))
            {
                return BadRequest("UserId is required.");
            }
            // Create BankAccount object
            var bankAccount = new BankAccount
            {
                UserId = createBankAccountRequest.UserId,
                Iban = createBankAccountRequest.Iban,
            };

            await _bankAccountService.CreateBankAccount(createBankAccountRequest);

            return Ok(); // No need to return any data
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("addfunds")]
    public async Task<IActionResult> AddFunds(AddFundsRequest addFundsRequest)
    {
        await _addFundsService.AddFunds(addFundsRequest);
        return Ok();
    }
}

