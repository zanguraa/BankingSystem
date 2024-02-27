﻿using BankingSystem.Core.Features.Cards;
using BankingSystem.Core.Features.Cards.CreateCard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BankingSystem.Api.Controllers
{
	[ApiController]
	[Route("api/cards")]
	public class CardController : ControllerBase
	{
		private readonly ICardService _cardService;

		public CardController(ICardService cardService)
		{
			_cardService = cardService ?? throw new ArgumentNullException(nameof(cardService));
		}

		[HttpPost("Create/cards")]
		public async Task<IActionResult> CreateCard([FromBody] CreateCardRequest createCardRequest)
		{
			var result = await _cardService.CreateCardAsync(createCardRequest);
			if (result == null) 
			{
				return BadRequest("Card didnt create");
			}
			return Ok(result); 
		}

		

		[HttpGet("user/{userId}")]
		public async Task<IActionResult> GetCardsByUserId(int userId)
		{
			try
			{
				var cards = await _cardService.GetCardsByUserIdAsync(userId);
				return Ok(cards);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}