using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BankingSystem.Core.Data
{
    public interface IDataManager
    {
        Task<int> Execute<T>(string sql, T item);
        Task<bool> ExecuteWithTransaction(List<SqlCommand> dataRequest);
        Task<IEnumerable<T>> Query<T, P>(string sql, P parameters);
        Task<IEnumerable<T>> Query<T>(string sql);
    }

    public class DataManager : IDataManager
    {
        private readonly string _connectionString;
        public DataManager(IConfiguration configuration)
        {
            string username = Environment.UserName;

            _connectionString = configuration.GetConnectionString(username);
        }
        public async Task<int> Execute<T>(string sql, T item)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync(sql, item);
        }
        public async Task<IEnumerable<T>> Query<T, P>(string sql, P parameters)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<T>(sql, parameters);
        }

        public async Task<IEnumerable<T>> Query<T>(string sql)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<T>(sql);
        }

        public async Task<bool> ExecuteWithTransaction(List<SqlCommand> dataRequest)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                foreach (var request in dataRequest)
                {
                    var result = await connection.ExecuteAsync(request.Query, request.Params, transaction);
                    if (result == 0)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                }
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}
