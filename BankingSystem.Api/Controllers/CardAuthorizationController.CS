using BankingSystem.Core.Features.Atm.CardAuthorization;
using BankingSystem.Core.Features.Atm.CardAuthorizations.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class CardAuthorizationController : ControllerBase
{
	private readonly ICardAuthorizationService _cardAuthorizationService;

	public CardAuthorizationController(ICardAuthorizationService cardAuthorizationService)
	{
		_cardAuthorizationService = cardAuthorizationService;
	}

	[HttpPost("Authorize")]
	public async Task<IActionResult> Authorize([FromBody] CardAuthorizationRequest request)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		try
		 {
			var isAuthorized = await _cardAuthorizationService.AuthorizeCardAsync(request);

			var responseDto = new CardAuthorizationResponse
			{
				IsAuthorized = isAuthorized,
				Message = isAuthorized ? "Authorization successful." : "Authorization failed."
			};

			return Ok(responseDto);
		}
		catch (Exception ex)
		{
			return StatusCode(500, "An error occurred while processing your request.");
		}
	}
}

