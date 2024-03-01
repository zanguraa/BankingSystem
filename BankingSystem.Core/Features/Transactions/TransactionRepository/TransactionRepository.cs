using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BankingSystem.Core.Features.Transactions;
using BankingSystem.Core.Features.Transactions.TransactionRepository;

public class TransactionRepository : ITransactionRepository
{
	private readonly string _connectionString;

	public TransactionRepository(string connectionString)
	{
		_connectionString = connectionString;
	}

	public Task<int> AddTransactionAsync(Transaction transaction)
	{
		throw new NotImplementedException();
	}

	public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
	{
		using (var connection = new SqlConnection(_connectionString))
		{
			connection.Open();
			var command = connection.CreateCommand();
			command.CommandType = CommandType.Text;
			command.CommandText = @"
                INSERT INTO Transactions (FromAccountId, ToAccountId, FromAccountCurrency, ToAccountCurrency, FromAmount, ToAmount, Fee, TransactionDate)
                VALUES (@FromAccountId, @ToAccountId, @FromAccountCurrency, @ToAccountCurrency, @FromAmount, @ToAmount, @Fee, @TransactionDate);
                SELECT SCOPE_IDENTITY();";

			command.Parameters.AddWithValue("@FromAccountId", transaction.FromAccountId);
			command.Parameters.AddWithValue("@ToAccountId", transaction.ToAccountId);
			command.Parameters.AddWithValue("@FromAccountCurrency", transaction.FromAccountCurrency);
			command.Parameters.AddWithValue("@ToAccountCurrency", transaction.ToAccountCurrency);
			command.Parameters.AddWithValue("@FromAmount", transaction.FromAmount);
			command.Parameters.AddWithValue("@ToAmount", transaction.ToAmount);
			command.Parameters.AddWithValue("@Fee", transaction.Fee);
			command.Parameters.AddWithValue("@TransactionDate", transaction.TransactionDate);

			// Execute the command and get the inserted ID back
			var result = await command.ExecuteScalarAsync();
			transaction.TransactionId = Convert.ToInt32(result);

			return transaction;
		}
	}

	public async Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId)
	{
		var transactions = new List<Transaction>();

		using (var connection = new SqlConnection(_connectionString))
		{
			connection.Open();
			var command = connection.CreateCommand();
			command.CommandType = CommandType.Text;
			command.CommandText = @"
                SELECT TransactionId, FromAccountId, ToAccountId, FromAccountCurrency, ToAccountCurrency, FromAmount, ToAmount, Fee, TransactionDate
                FROM Transactions
                WHERE FromAccountId = @AccountId OR ToAccountId = @AccountId";

			command.Parameters.AddWithValue("@AccountId", accountId);

			using (var reader = await command.ExecuteReaderAsync())
			{
				while (await reader.ReadAsync())
				{
					transactions.Add(new Transaction
					{
						TransactionId = reader.GetInt32(reader.GetOrdinal("TransactionId")),
						FromAccountId = reader.GetInt32(reader.GetOrdinal("FromAccountId")),
						ToAccountId = reader.GetInt32(reader.GetOrdinal("ToAccountId")),
						FromAccountCurrency = reader.GetString(reader.GetOrdinal("FromAccountCurrency")),
						ToAccountCurrency = reader.GetString(reader.GetOrdinal("ToAccountCurrency")),
						FromAmount = reader.GetDecimal(reader.GetOrdinal("FromAmount")),
						ToAmount = reader.GetDecimal(reader.GetOrdinal("ToAmount")),
						Fee = reader.GetDecimal(reader.GetOrdinal("Fee")),
						TransactionDate = reader.GetDateTime(reader.GetOrdinal("TransactionDate"))
					});
				}
			}
		}

		return transactions;
	}

	
	public async Task<bool> UpdateTransactionAsync(Transaction transaction)
	{
		using (var connection = new SqlConnection(_connectionString))
		{
			var command = new SqlCommand("UPDATE Transactions SET FromAccountId = @FromAccountId, ToAccountId = @ToAccountId, FromAccountCurrency = @FromAccountCurrency, ToAccountCurrency = @ToAccountCurrency, FromAmount = @FromAmount, ToAmount = @ToAmount, Fee = @Fee, TransactionDate = @TransactionDate WHERE TransactionId = @TransactionId", connection);

			command.Parameters.AddWithValue("@TransactionId", transaction.TransactionId);
			command.Parameters.AddWithValue("@FromAccountId", transaction.FromAccountId);
			command.Parameters.AddWithValue("@ToAccountId", transaction.ToAccountId);
			command.Parameters.AddWithValue("@FromAccountCurrency", transaction.FromAccountCurrency);
			command.Parameters.AddWithValue("@ToAccountCurrency", transaction.ToAccountCurrency);
			command.Parameters.AddWithValue("@FromAmount", transaction.FromAmount);
			command.Parameters.AddWithValue("@ToAmount", transaction.ToAmount);
			command.Parameters.AddWithValue("@Fee", transaction.Fee);
			command.Parameters.AddWithValue("@TransactionDate", transaction.TransactionDate);

			await connection.OpenAsync();
			var result = await command.ExecuteNonQueryAsync();
			return result > 0;
		}
	}

	public async Task<bool> DeleteTransactionAsync(int transactionId)
	{
		using (var connection = new SqlConnection(_connectionString))
		{
			var command = new SqlCommand("DELETE FROM Transactions WHERE TransactionId = @TransactionId", connection);
			command.Parameters.AddWithValue("@TransactionId", transactionId);

			await connection.OpenAsync();
			var result = await command.ExecuteNonQueryAsync();
			return result > 0;
		}
	}

	public async Task<Transaction> GetTransactionByIdAsync(int transactionId)
	{
		Transaction transaction = null;

		using (var connection = new SqlConnection(_connectionString))
		{
			var command = new SqlCommand("SELECT TransactionId, FromAccountId, ToAccountId, FromAccountCurrency, ToAccountCurrency, FromAmount, ToAmount, Fee, TransactionDate FROM Transactions WHERE TransactionId = @TransactionId", connection);
			command.Parameters.AddWithValue("@TransactionId", transactionId);

			await connection.OpenAsync();
			using (var reader = await command.ExecuteReaderAsync())
			{
				if (await reader.ReadAsync())
				{
					transaction = new Transaction
					{
						TransactionId = reader.GetInt32(reader.GetOrdinal("TransactionId")),
						FromAccountId = reader.GetInt32(reader.GetOrdinal("FromAccountId")),
						ToAccountId = reader.GetInt32(reader.GetOrdinal("ToAccountId")),
						FromAccountCurrency = reader.GetString(reader.GetOrdinal("FromAccountCurrency")),
						ToAccountCurrency = reader.GetString(reader.GetOrdinal("ToAccountCurrency")),
						FromAmount = reader.GetDecimal(reader.GetOrdinal("FromAmount")),
						ToAmount = reader.GetDecimal(reader.GetOrdinal("ToAmount")),
						Fee = reader.GetDecimal(reader.GetOrdinal("Fee")),
						TransactionDate = reader.GetDateTime(reader.GetOrdinal("TransactionDate")),
					};
				}
			}
		}

		return transaction;
	}
	Task<List<Transaction>> ITransactionRepository.GetTransactionsByAccountIdAsync(int accountId)
	{
		throw new NotImplementedException();
	}
}