using BankingSystem.Core.Data;
using BankingSystem.Core.Features.BankAccounts.AddFunds.Models.Requests;
using BankingSystem.Core.Features.BankAccounts.Requests;
using BankingSystem.Core.Features.Transactions;
using BankingSystem.Core.Features.Transactions.CreateTransactions;
using BankingSystem.Core.Features.Transactions.TransactionServices;

namespace BankingSystem.Core.Features.BankAccounts.AddFunds
{
    public interface IAddFundsRepository
    {
        Task<bool> AddFunds(AddFundsRequest addFundsRequest);
        Task<bool> ProcessDepositTransactionAsync(Transaction transactionRequest);
    }

    public class AddFundsRepository : IAddFundsRepository
    {
        private readonly IDataManager _dataManager;

        public AddFundsRepository(IDataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public async Task<bool> AddFunds(AddFundsRequest addFundsRequest)
        {
            string query = "UPDATE BankAccounts SET InitialAmount = InitialAmount + @Amount WHERE Id = @BankAccountId";
            var result = await _dataManager.Execute(query, new { addFundsRequest.BankAccountId, addFundsRequest.Amount });
            if (result > 0)
            {
                var logDepositRequest = new CreateTransactionRequest
                {
                    ToAccountId = addFundsRequest.BankAccountId,
                    Amount = addFundsRequest.Amount
                };
                return true;
            }
            return false;
        }

        public async Task<bool> ProcessDepositTransactionAsync(Transaction transactionRequest)
        {
            var SqlCommandList = new List<SqlCommand>
             {
                new() {
                    Query = @"
                        UPDATE BankAccounts
                        SET InitialAmount = InitialAmount + @ToAmount 
                        WHERE Id = @ToAccountId",
                    Params = transactionRequest
                },
                new() {
                    Query = @"
                        INSERT INTO Transactions (FromAccountId, ToAccountId, FromAccountCurrency, ToAccountCurrency, FromAmount, ToAmount, TransactionDate, TransactionType, Fee)
                        VALUES (@FromAccountId, @ToAccountId, @FromAccountCurrency, @ToAccountCurrency, @FromAmount, @ToAmount, @TransactionDate, @TransactionType, @Fee);",
                    Params = transactionRequest
                }
            };
            bool success = await _dataManager.ExecuteWithTransaction(SqlCommandList);

            if (!success) throw new Exception("An error occurred while processing your request.");

            return success;
        }
    }
}
