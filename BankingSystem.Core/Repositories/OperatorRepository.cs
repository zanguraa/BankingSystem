﻿using BankingSystem.Core.Interfaces;
using BankingSystem.Core.Models;
using BankingSystem.Core.Models.Requests;
using BankingSystem.Core.Shared;
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
            request.Password = PasswordHasher.HashHmacSHA256(request.Password);
            string sql = "INSERT INTO Operator ( Username, Password) VALUES (@Username, @Password)";
            return _dataManager.Execute(sql, request) > 0;
        }

        public bool OperatorExists(string username)
        {
          string sql = "SELECT * FROM Operator WHERE Username = @Username";

            var data = _dataManager.Query<Operator, object>(sql, new { Username = username });

            return data.FirstOrDefault() != null;
        }
    }
}
