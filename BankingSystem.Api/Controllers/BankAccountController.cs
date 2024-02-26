using BankingSystem.Core.Features.BankAccounts.CreateBankAccount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class BankAccountController : ControllerBase
{
    private readonly IBankAccountRepository _bankAccountRepository;

    public BankAccountController(IBankAccountRepository bankAccountRepository)
    {
        _bankAccountRepository = bankAccountRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBankAccount(CreateBankAccountRequest createBankAccountRequest)
    {
        try
        {
            if (createBankAccountRequest == null || createBankAccountRequest.UserId == default(int))
            {
                return BadRequest("UserId is required.");
            }

            if (createBankAccountRequest.InitialAmount <= 0)
            {
                return BadRequest("InitialAmount must be greater than zero.");
            }

            // Validate currency
            if (!Enum.IsDefined(typeof(BankingSystem.Core.Features.BankAccounts.CreateBankAccount.CurrencyType), createBankAccountRequest.Currency))
            {
                return BadRequest($"Invalid currency: {createBankAccountRequest.Currency}");
            }

            // Additional business logic validations can be added here

            string countryCode = "GE";
            string bankInitials = "CD";
            string randomBban = IbanGenerator.GenerateRandomNumeric(16);
            var iban = IbanGenerator.GenerateIban(countryCode, bankInitials, randomBban);

            // Create BankAccount object
            var bankAccount = new BankAccount
            {
                UserId = createBankAccountRequest.UserId,
                Iban = iban,
                InitialAmount = createBankAccountRequest.InitialAmount,
                Currency = createBankAccountRequest.Currency
            };

            // Call repository to create bank account
            await _bankAccountRepository.CreateBankAccountAsync(bankAccount);

            return Ok(); // No need to return any data
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }



    [HttpGet]
    public async Task<IActionResult> GetBankAccounts()
    {
        try
        {
            var bankAccounts = await _bankAccountRepository.GetBankAccounts();
            return Ok(bankAccounts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    
}

