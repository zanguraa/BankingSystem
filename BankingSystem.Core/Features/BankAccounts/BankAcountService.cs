using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankingSystem.Core.Data;
using BankingSystem.Core.Features.BankAccounts;
using BankingSystem.Core.Features.BankAccounts.Requests;
using BankingSystem.Core.Features.Transactions.TransactionsRepository;

public class BankAccountService : IBankAccountService
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ITransactionRepository _transactionRepository;


    public BankAccountService(IBankAccountRepository bankAccountRepository, ITransactionRepository transactionRepository)
    {
        _bankAccountRepository = bankAccountRepository ?? throw new ArgumentNullException(nameof(bankAccountRepository));
        _transactionRepository = transactionRepository;
    }

    public async Task<List<int>> CreateBankAccount(CreateBankAccountRequest createBankAccountRequest)
    {

        
        var iban = IbanGenerator.GenerateIban();

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

    public async Task<bool> CheckAccountOwnershipAsync(int accountId, string userId)
    {
        var isCorrectAccount = await _transactionRepository.CheckAccountOwnershipAsync(accountId, userId);
        if(!isCorrectAccount)
        {
            throw new Exception("You do not have permission to access this account.");
        }

        return isCorrectAccount;
    }

    public async Task<bool> ValidateAccountAsync(int accountId)
	{
		return await _bankAccountRepository.ContainsAccountAsync(accountId);
	}
}
	
