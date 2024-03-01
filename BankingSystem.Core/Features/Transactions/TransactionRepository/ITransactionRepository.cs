namespace BankingSystem.Core.Features.Transactions.TransactionRepository
{
    public interface ITransactionRepository
    {
        Task<int> AddTransactionAsync(Transaction transaction);
		/// ქმნის ახალ ტრანზაქციას ასინქრონულად.
		Task<Transaction> CreateTransactionAsync(Transaction transaction);
		/// ამოიღებს ტრანზაქციას მისი ID-ით ასინქრონულად.
		Task<List<Transaction>> GetTransactionsByAccountIdAsync(int accountId);

		/// ამოიღებს ყველა ტრანზაქციას კონკრეტული ანგარიშის ID-ისთვის ასინქრონულად.
		
		Task<bool> UpdateTransactionAsync(Transaction transaction);

		Task<bool> DeleteTransactionAsync(int transactionId);

        Task<Transaction> GetTransactionByIdAsync(int transactionId);

	}
}