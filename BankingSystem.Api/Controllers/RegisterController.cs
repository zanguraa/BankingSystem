using BankingSystem.Core.Features.Users.CreateUser.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegisterController : ControllerBase
{

    private readonly IUserService _userService;

    public RegisterController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    [Authorize("OperatorPolicy", AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        await _userService.RegisterUser(request);

        return Ok();
    }

    [HttpGet]
    [Route("test-operator")]
    [Authorize("OperatorPolicy", AuthenticationSchemes = "Bearer")]
    public IActionResult TestOperator()
    {
        return Ok("ok");
    }
}
