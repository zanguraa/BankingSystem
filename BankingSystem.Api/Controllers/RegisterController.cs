using BankingSystem.Core.Data;
using BankingSystem.Core.Interfaces;
using BankingSystem.Core.Models.Requests;
using BankingSystem.Core.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {

        private readonly JwtTokenGenerator _JwtTokenGenerator;
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<RoleEntity> _roleManager;

        public RegisterController(JwtTokenGenerator JwtTokenGenerator, UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager)
        {
            _JwtTokenGenerator = JwtTokenGenerator;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        [Authorize("OperatorPolicy", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var guid = User.Claims.FirstOrDefault()?.Value;

            var entity = new UserEntity
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                BirthdayDate = request.BirthdayDate,
                PersonalId = request.PersonalId


            };

            // მომხმარებლის რეგისტრაცია
            var result = await _userManager.CreateAsync(entity, request.Password);

            if (!result.Succeeded)
            {
                var firstError = result.Errors.First();
                return BadRequest(firstError.Description);
            }

            // მომხმარებლისთვის api-user როლის მინიჭება
            var addToRoleResult = await _userManager.AddToRoleAsync(entity, "user");

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
}
