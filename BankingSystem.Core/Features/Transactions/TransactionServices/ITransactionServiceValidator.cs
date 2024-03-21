using BankingSystem.Core.Features.Transactions.CreateTransactions;

namespace BankingSystem.Core.Features.Transactions.TransactionServices
{
    public interface ITransactionServiceValidator
    {
        Task ValidateCreateTransactionRequest(CreateTransactionRequest request);
    }
}