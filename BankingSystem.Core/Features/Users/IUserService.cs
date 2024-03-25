using BankingSystem.Core.Data.Entities;
using BankingSystem.Core.Features.Users.CreateUser;

namespace BankingSystem.Core.Features.Users
{
    public interface IUserService
    {
        Task<UserEntity> RegisterUser(RegisterUserRequest registerRequest);
    }
}
