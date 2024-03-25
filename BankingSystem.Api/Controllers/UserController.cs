using BankingSystem.Core.Data.Entities;
using BankingSystem.Core.Features.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Api.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly UserManager<UserEntity> _userManager;
		private readonly IUserService _userService;
		private readonly ILogger<UserController> _logger;

		public UserController(UserManager<UserEntity> userManager, IUserService userService, ILogger<UserController> logger)
		{
			_userManager = userManager;
			_userService = userService ?? throw new ArgumentNullException(nameof(userService));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		[HttpPost("get-user-by-email")]
		public async Task<IActionResult> GetUserByEmail([FromBody] string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				return BadRequest("Email parameter is required");
			}
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				return NotFound("User not found");
			}

			var userDto = new UserDto
			{
				PersonalId = user.PersonalId,
				BirthdayDate = user.BirthdayDate,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				PhoneNumber = user.PhoneNumber
			};
			return Ok(userDto);
		}

	}
}
