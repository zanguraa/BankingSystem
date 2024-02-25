using BankingSystem.Core.Features.Cards;
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

		[HttpPost]
		public async Task<IActionResult> CreateCard([FromBody] CreateCardRequest createCardRequest)
		{
			try
			{
				var createdCard = await _cardService.CreateCardAsync(createCardRequest);
				return CreatedAtAction(nameof(GetCardByNumber), new { cardNumber = createdCard.CardNumber }, createdCard);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("{cardNumber}")]
		public async Task<IActionResult> GetCardByNumber(string cardNumber)
		{
			try
			{
				var card = await _cardService.GetCardByNumberAsync(cardNumber);
				if (card == null)
					return NotFound();

				return Ok(card);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
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