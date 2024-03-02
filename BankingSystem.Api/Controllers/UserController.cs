﻿using BankingSystem.Core.Data.Entities;
using BankingSystem.Core.Features.Users;
using BankingSystem.Core.Features.Users.CreateUser;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> GetUserByEmail([FromBody]string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email parameter is required");
            }

            try
            {
                var user = await _userManager.FindByEmailAsync(email);
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
