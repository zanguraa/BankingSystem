using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankingSystem.Core.Features.BankAccounts.CreateBankAccount;

public class BankAccountService
{
    private readonly IBankAccountRepository _bankAccountRepository;

    public BankAccountService(IBankAccountRepository bankAccountRepository)
    {
        _bankAccountRepository = bankAccountRepository ?? throw new ArgumentNullException(nameof(bankAccountRepository));
    }

    public async Task<Guid> CreateBankAccount(CreateBankAccountRequest createBankAccountRequest)
    {
        // Generate a unique identifier for the bank account
        Guid bankAccountId = Guid.NewGuid();

        // Construct the bank account object using the request data
        BankAccount bankAccount = new BankAccount
        {
            Id = bankAccountId,
            UserId = createBankAccountRequest.UserId,
            Iban = createBankAccountRequest.Iban,
            InitialAmount = createBankAccountRequest.InitialAmount,
            Currency = (BankAccount.CurrencyType)createBankAccountRequest.Currency
        };

        // Save the bank account using the repository
        await _bankAccountRepository.CreateBankAccountAsync(bankAccount);

        // Return the ID of the newly created bank account
        return bankAccountId;
    }

    public async Task<List<BankAccount>> GetBankAccounts()
    {
        return await _bankAccountRepository.GetBankAccounts();
    }
}
