using BankingSystem.Core.Features.Transactions.CreateTransaction;

namespace BankingSystem.Core.Features.Transactions.TransactionService
{
	public interface ITransactionService
	{
		Task<int> AddTransactionAsync(Transaction transaction);
		Task<List<Transaction>> GetTransactionsByAccountIdAsync(int accountId);
		Task<TransactionResponse> CreateTransactionAsync(CreateTransactionRequest request);
		
	}
}