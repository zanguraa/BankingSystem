﻿using BankingSystem.Core.Data;
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
				var jwt = _JwtTokenGenerator.Generate("1" , "user");
				return Ok(jwt);
			}

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var entity = new UserEntity
            {
                UserName = request.Email,
                Email = request.Email
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
			[Route("test")]
			[Authorize("MyApiUserPolicy", AuthenticationSchemes = "Bearer")]
			public IActionResult Test()
			{
				return Ok("ok");
			}

		}

	}

