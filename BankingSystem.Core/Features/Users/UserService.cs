﻿using Microsoft.AspNetCore.Identity;
using BankingSystem.Core.Data.Entities;
using BankingSystem.Core.Features.Users.CreateUser;
using BankingSystem.Core.Shared.Exceptions;
using System.Text.RegularExpressions;
using BankingSystem.Core.Features.Users.Authorization;
using BankingSystem.Core.Shared;

namespace BankingSystem.Core.Features.Users
{
    public interface IUserService
    {
        Task<string> AuthorizeUser(LoginRequest request);
        Task<UserEntity> RegisterUser(RegisterUserRequest registerRequest);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtGenerator;

        public UserService(UserManager<UserEntity> userManager, IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _jwtGenerator = jwtTokenGenerator;
        }

        public async Task<UserEntity> RegisterUser(RegisterUserRequest registerRequest)
        {
            ValidateRegisterRequest(registerRequest);

            var existingUserByEmail = await _userManager.FindByEmailAsync(registerRequest.Email);
            if (existingUserByEmail != null)
            {
                throw new UserValidationException("Email is already registered.");
            }

            var existingUserByPersonalId = await _userRepository.UserByPersonalIdExist(registerRequest.PersonalId);
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

            return newUser;
        }

        public async Task<string> AuthorizeUser(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email)
                ?? throw new UserNotFoundException("Email {UserEmail} or Password is inccorect", request.Email);


            var isCorrectPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isCorrectPassword)
            {
                throw new UserNotFoundException("Email {UserEmail} or Password is inccorect", request.Email);
            }

            var role = await _userManager.GetRolesAsync(user);

            var jwtTokken = _jwtGenerator.Generate(request.Email, "user");

            return jwtTokken;

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
        }

    }
}
