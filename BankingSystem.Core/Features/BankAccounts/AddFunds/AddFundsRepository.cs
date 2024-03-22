using BankingSystem.Core.Features.BankAccounts.Requests;

namespace BankingSystem.Core.Features.BankAccounts.AddFunds
{
    public interface IAddFundsRepository
    {
        Task<bool> AddFunds(AddFundsRequest addFundsRequest);
    }

    public class AddFundsRepository : IAddFundsRepository
    {

        public async Task<bool> AddFunds(AddFundsRequest addFundsRequest)
        {
            string query = "UPDATE BankAccounts SET InitialAmount = InitialAmount + @Amount WHERE Id = @BankAccountId";
            var result = await _dataManager.Execute(query, new { addFundsRequest.BankAccountId, addFundsRequest.Amount });
            if (result > 0)
            {
                var logDepositRequest = new LogDepositRequest
                {
                    BankAccountId = addFundsRequest.BankAccountId,
                    Amount = addFundsRequest.Amount
                };
                return await LogDeposit(logDepositRequest);
            }
            return false;
        }
    }
}
