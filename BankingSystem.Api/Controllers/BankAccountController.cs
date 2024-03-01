using BankingSystem.Core.Features.BankAccounts;
using BankingSystem.Core.Features.BankAccounts.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class BankAccountController : ControllerBase
{
    private readonly IBankAccountService _bankAccountService;

    public BankAccountController(IBankAccountService bankAccountService)
    {
        _bankAccountService = bankAccountService;
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
        try
        {
            await _bankAccountService.AddFunds(addFundsRequest);

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}

