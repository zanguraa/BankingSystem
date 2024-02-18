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
    public class DataManager : IDatamanager
    {
        private readonly string _connectionString = "Data Source=DESKTOP-7OLVUTI;Database=BankingSystem_db;Integrated Security=SSPI;TrustServerCertificate=True";

        public int Execute<T>(string sql, T item)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            return  connection.Execute(sql, item);
        }

        public IEnumerable<T> Query<T, P>(string sql, P parameters)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            return connection.Query<T>(sql, parameters);
        }
    }
}
