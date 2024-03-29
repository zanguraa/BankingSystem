using BankingSystem.Core.Features.Users.AuthorizeUser;
using BankingSystem.Core.Features.Users.AuthorizeUser.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthorizeUserService _authorizeUserService;

    public AuthController(
        IAuthorizeUserService authorizeUserService)

    {
        _authorizeUserService = authorizeUserService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authorizeUserService.AuthorizeUser(request);

        return Ok(result);
    }


    [HttpGet]
    [Route("test")]
    [Authorize("MyApiUserPolicy", AuthenticationSchemes = "Bearer")]
    public IActionResult Test()
    {
        return Ok("ok");
    }

    [HttpGet]
    [Route("test-operator")]
    [Authorize("OperatorPolicy", AuthenticationSchemes = "Bearer")]
    public IActionResult TestOperator()
    {
        return Ok("ok");
    }
}

