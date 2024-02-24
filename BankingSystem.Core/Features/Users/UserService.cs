using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.Core.Data.Entities;
using BankingSystem.Core.Features.Users.CreateUser;
using BankingSystem.Core.Features.BankAccounts.CreateBankAccount;

namespace BankingSystem.Core.Features.Users
{
    public class UserService
    {
        private readonly UserManager<UserEntity> _userManager;

        public UserService(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserEntity> RegisterUser(RegisterUserRequest registerRequest)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerRequest.Email);
            if (existingUser != null)
            {
                throw new ApplicationException("Email is already registered");
            }

            var newUser = new UserEntity
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                PersonalId = registerRequest.PersonalId, // Make sure to set PersonalNumber
                BirthdayDate = registerRequest.BirthdayDate,
                Email = registerRequest.Email,
            };

            var result = await _userManager.CreateAsync(newUser, registerRequest.Password);
            if (!result.Succeeded)
            {
                throw new ApplicationException("Failed to create user");
            }

            return newUser;
        }

        public async Task<UserEntity> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public Task RegisterUser(CreateBankAccountRequest registerRequest)
        {
            throw new NotImplementedException();
        }
    }
}
