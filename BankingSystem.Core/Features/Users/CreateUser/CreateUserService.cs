using Microsoft.AspNetCore.Identity;
using BankingSystem.Core.Data.Entities;
using BankingSystem.Core.Shared.Exceptions;
using System.Text.RegularExpressions;
using BankingSystem.Core.Features.Users.CreateUser.Models.Requests;
using BankingSystem.Core.Features.Users.CreateUser.Models.response;
using Azure.Core;

namespace BankingSystem.Core.Features.Users.CreateUser
{
    public interface ICreateUserService
    {
        Task<RegisteredUserResponse> RegisterUserAsync(RegisterUserRequest registerRequest);
    }

    public class CreateUserService : ICreateUserService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly ICreateUserRepository _userRepository;

        public CreateUserService(UserManager<UserEntity> userManager, ICreateUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<RegisteredUserResponse> RegisterUserAsync(RegisterUserRequest registerRequest)
        {
            ValidateRegisterRequest(registerRequest);

            var existingUserByEmail = await _userManager.FindByEmailAsync(registerRequest.Email);
            if (existingUserByEmail != null)
            {
                throw new UserValidationException("Email is already registered.");
            }

            var existingUserByPersonalId = await _userRepository.UserByPersonalIdExistAsync(registerRequest.PersonalId);
            if (existingUserByPersonalId)
            {
                throw new UserValidationException("Personal ID is already registered.");
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
                throw new ApplicationException("Failed to create user. " + result.Errors.First().Description);
            }
            await _userManager.AddToRoleAsync(newUser, "user");

            return registerRequest.RegisteredUserResponse();
        }

        private void ValidateRegisterRequest(RegisterUserRequest registerRequest)
        {
            if (registerRequest == null)
            {
                throw new ArgumentNullException(nameof(registerRequest), "Registration request cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(registerRequest.Email) || !Regex.IsMatch(registerRequest.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                throw new UserValidationException("Invalid email address.");
            }

            if (string.IsNullOrWhiteSpace(registerRequest.FirstName))
            {
                throw new UserValidationException("First name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(registerRequest.LastName))
            {
                throw new UserValidationException("Last name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(registerRequest.PersonalId) || registerRequest.PersonalId.Length != 11)
            {
                throw new UserValidationException("Personal ID must be exactly 11 characters long.");
            }

            if (string.IsNullOrWhiteSpace(registerRequest.PhoneNumber) || registerRequest.PhoneNumber.Length != 9)
            {
                throw new UserValidationException("Phone number must be exactly 9 characters long.");
            }

            if (registerRequest.BirthdayDate <= DateTime.MinValue || registerRequest.BirthdayDate >= DateTime.MaxValue)
            {
                throw new UserValidationException("Invalid birthday date.");
            }
            var currentYear = DateTime.UtcNow.Year;
            var birthYear = registerRequest.BirthdayDate.Year;

            if (currentYear - birthYear < 18)
            {
                throw new UserValidationException("New user must be eighteen years old!");
            }
            else if (currentYear - birthYear == 18)
            {
                var today = DateTime.UtcNow;
                var eighteenthBirthday = registerRequest.BirthdayDate.AddYears(18);

                if (eighteenthBirthday > today)
                {
                    throw new UserValidationException("New user must be eighteen years old!");
                }
            }
        }

    }
}
