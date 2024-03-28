using BankingSystem.Core.Data;
using BankingSystem.Core.Shared.Models;


namespace BankingSystem.Core.Features.BankAccounts.CreateAccount;

public interface ICreateBankAccountsRepository
{
    Task<List<string>> GetIbansByAccountIdsAsync(List<int> accountIds);
    Task<int> CreateBankAccountAsync(BankAccount bankAccount);
    Task<BankAccount?> GetAccountByIbanAsync(string iban);
    Task<BankAccount?> GetAccountByIdAsync(int AccountId);
}

public class CreateBankAccountsRepository : ICreateBankAccountsRepository
{
    private readonly IDataManager _dataManager;

    public CreateBankAccountsRepository(IDataManager dataManager)
    {
        _dataManager = dataManager;
    }

    public async Task<BankAccount?> GetAccountByIbanAsync(string iban)
    {
        string query = "SELECT * FROM BankAccounts WHERE Iban = @Iban";
        var result = await _dataManager.Query<BankAccount, dynamic>(query, new { iban });
        return result.FirstOrDefault();
    }

    public async Task<int> CreateBankAccountAsync(BankAccount bankAccount)
    {
        string query = @"
                INSERT INTO BankAccounts ( UserId, Iban, InitialAmount, Currency)
                VALUES (@UserId, @Iban, @InitialAmount, @Currency);";

        var result = await _dataManager.Execute(query, new
        {
            bankAccount.UserId,
            bankAccount.Iban,
            bankAccount.InitialAmount,
            Currency = bankAccount.Currency.ToString()
        });

        if (result == 0)
        {
            throw new Exception("Failed to create bank account");
        }

        var newBankAccount = await GetAccountByIbanAsync(bankAccount.Iban);
        if (newBankAccount == null)
        {
            throw new Exception("Failed to create bank account");
        }
        return newBankAccount.Id;
    }

    public async Task<List<string>> GetIbansByAccountIdsAsync(List<int> accountIds)
    {
        var ibans = await _dataManager.Query<string, dynamic>(
            "SELECT Iban FROM BankAccounts WHERE Id IN @AccountIds;", new { AccountIds = accountIds });

        return ibans.ToList();
    }

    public async Task<BankAccount?> GetAccountByIdAsync(int AccountId)
    {
        var account = await _dataManager.Query<BankAccount, dynamic>(
                       "SELECT * FROM BankAccounts WHERE Id = @AccountId", new { AccountId });
        return account.FirstOrDefault();
    }
}
