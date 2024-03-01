using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankingSystem.Core.Data;
using BankingSystem.Core.Features.BankAccounts;
using BankingSystem.Core.Features.BankAccounts.Requests;

public class BankAccountService : IBankAccountService
{
    private readonly IBankAccountRepository _bankAccountRepository;

   
    public BankAccountService(IBankAccountRepository bankAccountRepository)
    {
        _bankAccountRepository = bankAccountRepository ?? throw new ArgumentNullException(nameof(bankAccountRepository));
    }

    public async Task<List<int>> CreateBankAccount(CreateBankAccountRequest createBankAccountRequest)
    {

        string countryCode = "GE";
        string bankInitials = "CD";
        string randomBban = IbanGenerator.GenerateRandomNumeric(16);
        var iban = IbanGenerator.GenerateIban(countryCode, bankInitials, randomBban);


        var currencies = Enum.GetValues<CurrencyType>();
        var accountIds = new List<int>();
        foreach (var currency in currencies)
        {
            BankAccount bankAccount = new BankAccount
            {
                UserId = createBankAccountRequest.UserId,
                Iban = iban,
                Currency = currency
            };

            // Save the bank account using the repository
            var accountId = await _bankAccountRepository.CreateBankAccountAsync(bankAccount);
            accountIds.Add(accountId);

        }
        // Return the ID of the newly created bank account
        return accountIds;
    }

    public async Task<bool> AddFunds(AddFundsRequest addFundsRequest)
    {
        if (addFundsRequest == null || addFundsRequest.Amount == default || addFundsRequest.BankAccountId <= 0)
        {
            throw new Exception("Invalid request");
        }
        return await _bankAccountRepository.AddFunds(addFundsRequest);

    }
	public async Task<bool> ValidateAccountAsync(int accountId)
	{
		return await _bankAccountRepository.ContainsAccountAsync(accountId);
	}
}
	
