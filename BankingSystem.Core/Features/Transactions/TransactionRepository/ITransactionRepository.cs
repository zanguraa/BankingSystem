namespace BankingSystem.Core.Features.Transactions.TransactionRepository
{
    public interface ITransactionRepository
    {
        Task<int> AddTransactionAsync(Transaction transaction);
        Task<List<Transaction>> GetTransactionsByAccountIdAsync(int accountId);
    }
}