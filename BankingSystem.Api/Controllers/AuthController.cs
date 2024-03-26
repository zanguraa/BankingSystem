using BankingSystem.Core.Data.Entities;
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
        private readonly JwtTokenGenerator _JwtTokenGenerator;
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<RoleEntity> _roleManager;

        public AuthController(
            UserManager<UserEntity> userManager,
            RoleManager<RoleEntity> roleManager,
            JwtTokenGenerator JwtTokenGenerator)
        {
            _JwtTokenGenerator = JwtTokenGenerator;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var isCorrectPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isCorrectPassword)
            {
                return BadRequest("ელ.ფოსტა ან პაროლი არასწორია");
            }

            var role = await _userManager.GetRolesAsync(user);
            return Ok(_JwtTokenGenerator.Generate(user.Id.ToString(), role.FirstOrDefault() ?? ""));
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

