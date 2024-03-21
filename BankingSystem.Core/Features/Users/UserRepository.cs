using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Users.CreateUser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BankingSystem.Core.Features.Users
{
    public class UserRepository : IUserRepository
    {
        
        private readonly IDataManager _dataManager;
        public UserRepository( IDataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public async Task<bool> UserByPersonalIdExist(string personalId)
        {
            string sql = "SELECT TOP 1 1 FROM Users WHERE PersonalId = @PersonalId";

            var result = await _dataManager.Query<int, dynamic>(
                sql,
                new { PersonalId = personalId }
            );

            return result.Any();
        }

    }
}
