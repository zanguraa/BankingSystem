using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Transactions.TransactionServices;

namespace BankingSystem.Core.Features.Transactions.TransactionsRepositories
{
    public interface ITransactionRepository
    {
        Task<bool> CheckAccountOwnershipAsync(int accountId, string userId);
        Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId);
        Task<bool> IsCurrencyValid(string currencyCode);
        Task<bool> ProcessAtmTransaction(Transaction transaction);
        Task<bool> ProcessBankTransaction(Transaction transaction);
        Task<bool> ProcessDepositTransactionAsync(Transaction transactionRequest);
        Task<bool> UpdateAccountBalancesAsync(Transaction transaction, bool isAtmWithdrawal = false);
    }

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

        public async Task<bool> ProcessAtmTransaction(Transaction transactionRequest)
        {
            var SqlCommandList = new List<SqlCommand>
             {
                new() {
                    Query = @"
                        UPDATE BankAccounts
                        SET InitialAmount = InitialAmount - @Amount 
                        WHERE Id = @AccountId",
                    Params = new { AccountId = transactionRequest.FromAccountId, Amount = transactionRequest.FromAmount + transactionRequest.Fee}
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

        public async Task<bool> ProcessBankTransaction(Transaction transactionRequest)
        {
            if (transactionRequest.TransactionType == (int)TransactionType.External)
            {
                transactionRequest.FromAmount += transactionRequest.Fee;
            }

            var SqlCommandList = new List<SqlCommand>
             {
                new() {
                    Query = @"
                        UPDATE BankAccounts
                        SET InitialAmount = InitialAmount - @Amount 
                        WHERE Id = @FromAccountId",
                    Params = transactionRequest
                },
                new()
                {
                    Query = @"
                        UPDATE BankAccounts
                        SET InitialAmount = InitialAmount + @Amount
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


        public async Task<bool> UpdateAccountBalancesAsync(Transaction transaction, bool isAtmWithdrawal = false)
        {
            var sqlCommandRequests = new List<SqlCommand>
    {
        new SqlCommand
        {
            Query = @"
                UPDATE BankAccounts
                SET InitialAmount = InitialAmount - @Amount 
                WHERE Id = @AccountId",
            Params = new { AccountId = transaction.FromAccountId, Amount = transaction.FromAmount }
        },

        new SqlCommand
        {
            Query = @"
                INSERT INTO Transactions (FromAccountId, ToAccountId, FromAccountCurrency, ToAccountCurrency, FromAmount, ToAmount, TransactionDate, TransactionType, Fee)
                VALUES (@FromAccountId, @ToAccountId, @FromAccountCurrency, @ToAccountCurrency, @FromAmount, @ToAmount, @TransactionDate, @TransactionType, @Fee);",
            Params = transaction
        }
    };

            if (!isAtmWithdrawal && transaction.ToAccountId != null)
            {
                sqlCommandRequests.Add(new SqlCommand
                {
                    Query = @"
                UPDATE BankAccounts
                SET InitialAmount = InitialAmount + @Amount
                WHERE Id = @AccountId",
                    Params = new { AccountId = transaction.ToAccountId, Amount = transaction.ToAmount }
                });
            }

            bool success = await _dataManager.ExecuteWithTransaction(sqlCommandRequests);

            if (!success)
            {
                throw new Exception("An error occurred while processing your request.");
            }
            return success;
        }



        public async Task<bool> IsCurrencyValid(string currencyCode)
        {
            var query = "SELECT TOP 1 1 FROM [Currencies] WHERE [Code] = @CurrencyCode";


            var result = await _dataManager.Query<int, dynamic>(
                query,
                new { CurrencyCode = currencyCode }
            );

            return result.FirstOrDefault() > 0;
        }
    }
}
