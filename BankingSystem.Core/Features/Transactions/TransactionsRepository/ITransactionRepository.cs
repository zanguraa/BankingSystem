namespace BankingSystem.Core.Features.Transactions.TransactionsRepository
{
    public interface ITransactionRepository
    {
        Task<int> AddTransactionAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId);


    }
}