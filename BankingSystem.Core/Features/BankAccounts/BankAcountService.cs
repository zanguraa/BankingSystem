using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankingSystem.Core.Features.BankAccounts;
using BankingSystem.Core.Features.BankAccounts.CreateBankAccount;

public class BankAccountService : IBankAccountService
{
    private readonly IBankAccountRepository _bankAccountRepository;

    public BankAccountService(IBankAccountRepository bankAccountRepository)
    {
        _bankAccountRepository = bankAccountRepository ?? throw new ArgumentNullException(nameof(bankAccountRepository));
    }

    public async Task<int> CreateBankAccount(CreateBankAccountRequest createBankAccountRequest)
    {
        

        // Construct the bank account object using the request data
        BankAccount bankAccount = new BankAccount
        {
            UserId = createBankAccountRequest.UserId,
            Iban = createBankAccountRequest.Iban,
            InitialAmount = createBankAccountRequest.InitialAmount,
            Currency = createBankAccountRequest.Currency
        };

        // Save the bank account using the repository
       var result =  await _bankAccountRepository.CreateBankAccountAsync(bankAccount);

        // Return the ID of the newly created bank account
        return result;
    }

    public async Task<List<BankAccount>> GetBankAccounts()
    {
        return await _bankAccountRepository.GetBankAccounts();
    }
}
