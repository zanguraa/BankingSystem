using BankingSystem.Core.Features.BankAccounts.AddFunds;
using BankingSystem.Core.Features.BankAccounts.Requests;

namespace BankingSystem.Core.Features.BankAccounts.BankAccountsServices
{
    public interface IBankAccountService
    {
        Task<List<int>> CreateBankAccount(CreateBankAccountRequest createBankAccountRequest);
        Task<bool> AddFunds(AddFundsRequest addFundsRequest);
        Task<bool> ValidateAccountAsync(int accountId);
        Task<bool> CheckAccountOwnershipAsync(int accountId, string userId);
    }
}
