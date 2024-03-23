using BankingSystem.Core.Data;
using BankingSystem.Core.Features.BankAccounts.Requests;


namespace BankingSystem.Core.Features.BankAccounts.BankAccountRepositories;

public interface IBankAccountRepository
{
    Task<bool> ContainsAccountAsync(int accountId);
    Task<int> CreateBankAccountAsync(BankAccount bankAccount);
    Task<bool> ExistsWithCurrencyAsync(int userId, string currency);
    Task<BankAccount?> GetAccountByIbanAsync(string iban);
    Task<BankAccount?> GetAccountByIdAsync(int AccountId);
}

public class BankAccountRepository : IBankAccountRepository
{
    private readonly IDataManager _dataManager;

    public BankAccountRepository(IDataManager dataManager)
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

    public async Task<bool> ExistsWithCurrencyAsync(int userId, string currency)
    {
        string query = "SELECT COUNT(*) FROM BankAccounts WHERE UserId = @UserId AND Currency = @Currency";
        var count = await _dataManager.Query<int, dynamic>(query, new { UserId = userId, Currency = currency });
        return count.FirstOrDefault() > 0;
    }




    public async Task<bool> ContainsAccountAsync(int accountId)
    {
        var account = await _dataManager.Query<int, dynamic>(
            "SELECT * FROM BankAccounts WHERE Id = @AccountId", new { AccountId = accountId });
        return account.Count() > 0;
    }

    public async Task<BankAccount?> GetAccountByIdAsync(int AccountId)
    {
        var account = await _dataManager.Query<BankAccount, dynamic>(
                       "SELECT * FROM BankAccounts WHERE Id = @AccountId", new { AccountId });
        return account.FirstOrDefault();
    }
}
