using BankingSystem.Core.Features.Transactions.CreateTransactions;

namespace BankingSystem.Core.Features.Transactions.TransactionServices
{
    public interface ITransactionService
	{
        Task<int> AddTransactionAsync(Transaction transaction);
        Task<TransactionResponse> CreateTransactionAsync(CreateTransactionRequest request);
        Task<List<Transaction>> GetTransactionsByAccountIdAsync(int accountId);
	}
}