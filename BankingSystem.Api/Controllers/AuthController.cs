using BankingSystem.Core.Data.Entities;
using BankingSystem.Core.Features.Users;
using BankingSystem.Core.Features.Users.Authorization;
using BankingSystem.Core.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(
            IUserService userService
            )
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result =await _userService.AuthorizeUser(request);

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
}

