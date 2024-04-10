using BankingSystem.Core.Features.Users.CreateUser;
using BankingSystem.Core.Features.Users.CreateUser.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegisterController : ControllerBase
{

    private readonly ICreateUserService _createUserService;

    public RegisterController(ICreateUserService createUserService)
    {
        _createUserService = createUserService;
    }

    [HttpPost("register")]
    [Authorize("OperatorPolicy", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        await _createUserService.RegisterUserAsync(request);

        return Ok(request.RegisteredUserResponse());
    }

    [HttpGet]
    [Route("test-operator")]
    [Authorize("OperatorPolicy", AuthenticationSchemes = "Bearer")]
    public IActionResult TestOperator()
    {
        return Ok("ok");
    }
}
