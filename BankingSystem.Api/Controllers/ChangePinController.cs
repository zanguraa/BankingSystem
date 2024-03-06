﻿using BankingSystem.Core.Features.Atm.ChangePin;
using BankingSystem.Core.Features.Atm.ChangePin.Dto_s;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ChangePinController : ControllerBase
{
	private readonly IChangePinService _changePinService;

	public ChangePinController(IChangePinService changePinService)
	{
		_changePinService = changePinService;
	}

	[HttpPost("ChangePin")]
	public async Task<IActionResult> ChangePin([FromBody] ChangePinRequestDto request)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		try
		{
			var result = await _changePinService.ChangePinAsync(request.CardNumber, request.CurrentPin, request.NewPin);

			if (result)
			{
				return Ok(new ChangePinResponseDto { Success = true, Message = "PIN changed successfully." });
			}
			else
			{
				return BadRequest(new ChangePinResponseDto { Success = false, Message = "Failed to change PIN. Please check the current PIN and try again." });
			}
		}
		catch (Exception ex)
		{
			return StatusCode(500, new ChangePinResponseDto { Success = false, Message = "An internal error occurred. Please try again later." });
		}
	}
}