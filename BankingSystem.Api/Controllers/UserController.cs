using BankingSystem.Core.Features.BankAccounts.CreateBankAccount;
using BankingSystem.Core.Features.Users;
using BankingSystem.Core.Features.Users.CreateUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Api.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly UserService _userService;
		private readonly ILogger<UserController> _logger;

		public UserController(UserService userService, ILogger<UserController> logger)
		{
			_userService = userService ?? throw new ArgumentNullException(nameof(userService));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterUserRequest registerRequest)
		{
			try
			{
				var newUser = await _userService.RegisterUser(registerRequest);
				return Ok(newUser);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error during user registration: {ex.Message}");
				return StatusCode(500, "Internal Server Error");
			}
		}

		[HttpGet("{email}")]
		public async Task<IActionResult> GetUserByEmail(string email)
		{
			try
			{
				var user = await _userService.GetUserByEmail(email);
				if (user == null)
				{
					return NotFound("User not found");
				}

				return Ok(user);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error fetching user details: {ex.Message}");
				return StatusCode(500, "Internal Server Error");
			}
		}
	}
}
