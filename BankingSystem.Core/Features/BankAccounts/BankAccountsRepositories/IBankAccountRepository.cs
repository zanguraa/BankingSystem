using BankingSystem.Core.Features.BankAccounts.Requests;

namespace BankingSystem.Core.Features.BankAccounts.BankAccountRepositories;
    public interface IBankAccountRepository
    {
        Task<BankAccount?> GetAccountByIbanAsync(string iban);
        Task<int> CreateBankAccountAsync(BankAccount bankAccount);
        Task<bool> AddFunds(AddFundsRequest addFundsRequest);
        Task<bool> ExistsWithCurrencyAsync(int userId, string currency);
        Task<bool> ContainsAccountAsync(int accountId);
        Task<BankAccount?> GetAccountByIdAsync(int AccountId);

    }
