using BankingSystem.Core.Data;
using BankingSystem.Core.Shared.Models;

namespace BankingSystem.Core.Features.BankAccounts.AddFunds;

public interface IAddFundsRepository
{
    Task<bool> ProcessDepositTransactionAsync(Transaction transactionRequest);
}

public class AddFundsRepository : IAddFundsRepository
{
    private readonly IDataManager _dataManager;

    public AddFundsRepository(IDataManager dataManager)
    {
        _dataManager = dataManager;
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
