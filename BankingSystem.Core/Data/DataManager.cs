using BankingSystem.Core.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Data
{
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
    }
}
