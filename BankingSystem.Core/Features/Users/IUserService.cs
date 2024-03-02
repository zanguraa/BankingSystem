using BankingSystem.Core.Data.Entities;
using BankingSystem.Core.Features.BankAccounts.Requests;
using BankingSystem.Core.Features.Users.CreateUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Users
{
    public interface IUserService
    {
        Task<UserEntity> RegisterUser(RegisterUserRequest registerRequest);
        Task<UserEntity> GetUserByEmail(string email);
        Task RegisterUser(CreateBankAccountRequest registerRequest);
    }
}
