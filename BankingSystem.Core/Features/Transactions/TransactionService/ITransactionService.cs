namespace BankingSystem.Core.Features.Transactions.TransactionService
{
	public interface ITransactionService
	{
		Task<int> AddTransactionAsync(Transaction transaction);
		Task<List<Transaction>> GetTransactionsByAccountIdAsync(int accountId);
	}
}