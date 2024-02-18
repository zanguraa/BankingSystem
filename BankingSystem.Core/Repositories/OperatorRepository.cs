using BankingSystem.Core.Interfaces;
using BankingSystem.Core.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Repositories
{
    public class OperatorRepository : IOperatorRepository
    {
        private readonly IDatamanager _dataManager;

        public OperatorRepository(IDatamanager datamanager)
        {
            _dataManager = datamanager;
        }

        public bool AddOperator(CreateOperatorRequest request)
        {
            string sql = "INSERT INTO Operator ( Username, Password) VALUES (@Username, @Password)";
            return _dataManager.Execute(sql, request) > 0;
        }

    }
}
