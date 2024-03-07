using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Transactions;
using BankingSystem.Core.Features.Transactions.TransactionsRepository;
using Dapper;
using Microsoft.EntityFrameworkCore;

public class TransactionRepository : ITransactionRepository
{
    private readonly IDataManager _dataManager;

    public TransactionRepository(IDataManager dataManager)
    {
        _dataManager = dataManager;
    }


    public async Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId)
    {
        string query = @"
            SELECT * FROM Transactions
            WHERE FromAccountId = @AccountId OR ToAccountId = @AccountId";

        var transactions = await _dataManager.Query<Transaction, dynamic>(query, new { AccountId = accountId });
        return transactions;
    }

    public async Task<bool> CheckAccountOwnershipAsync(int accountId, string userId)
    {
        var sql = @"SELECT COUNT(1) FROM [BankingSystem_db].[dbo].[BankAccounts] WHERE Id = @AccountId AND UserId = @UserId";
        var parameters = new { AccountId = accountId, UserId = userId };
        var count = await _dataManager.Query<int, dynamic>(sql, parameters);
        return count.FirstOrDefault() > 0;
    }

    public async Task UpdateAccountBalancesAsync(Transaction transaction)
    {

        SqlCommandRequest decreasAmountFromAccountIdCommand = new()
        {
            Query = @"
                UPDATE BankAccounts
                SET InitialAmount = InitialAmount - @FromAmount 
                WHERE Id = @FromAccountId",
            Params = new { transaction.FromAccountId, transaction.FromAmount }
        };

        SqlCommandRequest increaseAmountToAccountIdCommand = new()
        {
            Query = @"
                UPDATE BankAccounts
                SET InitialAmount = InitialAmount + @ToAmount
                WHERE Id = @ToAccountId",
            Params = new { transaction.ToAccountId, transaction.ToAmount }
        };

        SqlCommandRequest insertTransactionLogCommand = new()
        {
            Query = @"
            INSERT INTO Transactions (FromAccountId, ToAccountId, FromAccountCurrency, ToAccountCurrency, FromAmount, ToAmount, TransactionDate, TransactionType, Fee)
            VALUES (@FromAccountId, @ToAccountId, @FromAccountCurrency, @ToAccountCurrency, @FromAmount, @ToAmount, @TransactionDate, @TransactionType, @Fee);",
            Params = transaction
        };

        var sqlCommandRequests = new List<SqlCommandRequest>
        {
            decreasAmountFromAccountIdCommand,
            increaseAmountToAccountIdCommand,
            insertTransactionLogCommand
        };

        bool success = await _dataManager.ExecuteWithTransaction(sqlCommandRequests);

        if (!success)
        {
            throw new Exception("An error occurred while processing your request.");
        }
    }
}