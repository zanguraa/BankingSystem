namespace BankingSystem.Core.Features.Transactions.TransactionsRepository
{
    public interface ITransactionRepository
    {
        Task<int> AddTransactionAsync(Transaction transaction);
        Task<Transaction> GetTransactionByIdAsync(int transactionId);

	}
}