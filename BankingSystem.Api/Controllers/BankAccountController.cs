using BankingSystem.Core.Features.BankAccounts.CreateBankAccount;
using Microsoft.AspNetCore.Mvc;
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
            // Convert Currency string to CurrencyType enum
            if (!Enum.TryParse(createBankAccountRequest.Currency.ToString(), out BankAccount.CurrencyType currency))
            {
                throw new ArgumentException($"Invalid currency: {createBankAccountRequest.Currency}");
            }

            // Create BankAccount object
            var bankAccount = new BankAccount
            {
                UserId = createBankAccountRequest.UserId,
                Iban = createBankAccountRequest.Iban,
                InitialAmount = createBankAccountRequest.InitialAmount,
                Currency = currency
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

