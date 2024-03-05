using BankingSystem.Core.Features.Transactions.CreateTransactions;

namespace BankingSystem.Core.Features.Transactions.TransactionServices
{
    public interface ITransactionService
	{
        Task<TransactionResponse> TransferTransactionAsync(CreateTransactionRequest request);
        Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId);

    }
}