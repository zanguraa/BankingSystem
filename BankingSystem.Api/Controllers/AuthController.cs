using BankingSystem.Core.Data;
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

			[HttpGet]
			[Route("test")]
			[Authorize("MyApiUserPolicy", AuthenticationSchemes = "Bearer")]
			public IActionResult Test()
			{
				return Ok("ok");
			}

		}

	}

