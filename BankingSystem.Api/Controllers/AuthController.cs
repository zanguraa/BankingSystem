using BankingSystem.Core.Data;
using BankingSystem.Core.Models.Requests;
using BankingSystem.Core.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
		[HttpGet]
		[Route("generate-token")]
		public IActionResult GenerateToken()
		{
			var jwt = _JwtTokenGenerator.Generate("1", "user");
			return Ok(jwt);
		}

		// ავტორიზაცია
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null)
			{
				// Handle the case where the user is not found
				return NotFound("User not found.");
			}

			var isCorrectPassword = await _userManager.CheckPasswordAsync(user, request.Password);
			if (!isCorrectPassword)
			{
				return BadRequest("ელ.ფოსტა ან პაროლი არასწორია");
			}

			var role = await _userManager.GetRolesAsync(user);
			// var isOperator = await _userManager.IsInRoleAsync(user, "operator");
			return Ok(_JwtTokenGenerator.Generate(user.Id.ToString(), role.FirstOrDefault() ?? ""));
		}

		// პაროლის დარესეტების token-ის გენერაცია
		[HttpPost("request-password-reset")]
		public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetRequest request)
		{
			// 1. მომხმარებლის ბაზაში პოვნა 
			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null)
			{
				return NotFound("მომხმარებელი ვერ მოიძებნა");
			}

			// 2. პაროლის დარესეტების token-ის გენერაცია
			var token = await _userManager.GeneratePasswordResetTokenAsync(user);

			// // 3. მომხმარებლის ელ. ფოსტაზე token-ის გაგზავნა
			// var url = $"https://myapp.com/reset-passowrd/{user.Id.ToString()}/{token}";
			// var emailBody = $"<a href=\"{url}\">Reset password</a>";
			// var emailTitle = $"გამარჯობა, პაროლის შესაცვლელად მიყევით ბმულს: {resetUrl}";
			// _emailSender.Send(emailTitle, emailBody);

			return Ok();
		}

		// პაროლის დარესეტება
		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
		{
			var user = await _userManager.FindByIdAsync(request.UserId.ToString());
			if (user == null)
			{
				return NotFound("მომხმარებელი ვერ მოიძებნა");
			}
			var resetResult = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

			if (!resetResult.Succeeded)
			{
				var firstError = resetResult.Errors.First();
				return StatusCode(500, firstError.Description);
			}

			return Ok();
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

