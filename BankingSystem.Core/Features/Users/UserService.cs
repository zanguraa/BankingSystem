using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.Core.Data.Entities;
using BankingSystem.Core.Features.Users.CreateUser;
using BankingSystem.Core.Features.BankAccounts.Requests;
using Azure.Core;
using BankingSystem.Core.Shared.Exceptions;

namespace BankingSystem.Core.Features.Users
{
    public class UserService : IUserService
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
                UserName = registerRequest.Email,
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                PhoneNumber = registerRequest.PhoneNumber,
                BirthdayDate = registerRequest.BirthdayDate,
                PersonalId = registerRequest.PersonalId
            };

            

            var result = await _userManager.CreateAsync(newUser, registerRequest.Password);
            if (!result.Succeeded)
            {
                throw new ApplicationException("Failed to create user");
            }
            var addToRoleResult = await _userManager.AddToRoleAsync(newUser, "user");


            if (!result.Succeeded)
            {
                var firstError = result.Errors.First();
                throw new DomainException(firstError.Description);
            }

            return newUser;
        }

    }
}
