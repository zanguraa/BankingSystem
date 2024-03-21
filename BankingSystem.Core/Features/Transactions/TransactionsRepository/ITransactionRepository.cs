namespace BankingSystem.Core.Features.Transactions.TransactionsRepository
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId);
        Task<bool> CheckAccountOwnershipAsync(int accountId, string userId);
        Task<bool> UpdateAccountBalancesAsync(Transaction transaction, bool isAtmWithdrawal = false);
        Task<bool> IsCurrencyValid(string currencyCode);
    }
}