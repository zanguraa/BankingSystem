using BankingSystem.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Data
{
    public class DataManager : IDatamanager
    {
        private readonly string _connectionString = string.Empty;
        public DataManager(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default") ?? throw new ApplicationException("Connection string not found!");
        }
    }
}
