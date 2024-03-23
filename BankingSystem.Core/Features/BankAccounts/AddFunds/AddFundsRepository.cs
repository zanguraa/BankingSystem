using BankingSystem.Core.Data;
using BankingSystem.Core.Features.BankAccounts.Requests;
using BankingSystem.Core.Features.Transactions.CreateTransactions;
using BankingSystem.Core.Features.Transactions.TransactionServices;

namespace BankingSystem.Core.Features.BankAccounts.AddFunds
{
    public interface IAddFundsRepository
    {
        Task<bool> AddFunds(AddFundsRequest addFundsRequest);
    }

    public class AddFundsRepository : IAddFundsRepository
    {
        private readonly IDataManager _dataManager;
        private readonly ITransactionService _transactionService;

        public AddFundsRepository(IDataManager dataManager, ITransactionService transactionService)
        {
            _dataManager = dataManager;
            _transactionService = transactionService;
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
    }
}
