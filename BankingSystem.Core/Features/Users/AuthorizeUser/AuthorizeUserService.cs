using BankingSystem.Core.Data.Entities;
using BankingSystem.Core.Features.Users.AuthorizeUser.Requests;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace BankingSystem.Core.Features.Users.AuthorizeUser;

public interface IAuthorizeUserService
{
    Task<string> AuthorizeUser(LoginRequest request);
}

public class AuthorizeUserService : IAuthorizeUserService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthorizeUserService(UserManager<UserEntity> userManager, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    public async Task<string> AuthorizeUser(LoginRequest request)
    {
        var user = await ValidateUserCredentialsAsync(request.Email, request.Password);

        var isCorrectPassword = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isCorrectPassword)
        {
            throw new UserNotFoundException("Email {UserEmail} or Password is inccorect", request.Email);
        }

        var roles = await _userManager.GetRolesAsync(user);
        var jwtToken = _jwtTokenGenerator.Generate(request.Email, roles.FirstOrDefault());

        return jwtToken;
    }

    private async Task<UserEntity> ValidateUserCredentialsAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email)
            ?? throw new UserNotFoundException($"Email {email} or Password is incorrect", email);

        var isCorrectPassword = await _userManager.CheckPasswordAsync(user, password);
        if (!isCorrectPassword)
        {
            throw new UserNotFoundException($"Email {email} or Password is incorrect", email);
        }

        return user;
    }
}
