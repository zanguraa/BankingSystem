using BankingSystem.Core.Features.Atm.ViewBalance;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ViewBalanceController : ControllerBase
{
	private readonly IViewBalanceService _viewBalanceService;

	public ViewBalanceController(IViewBalanceService viewBalanceService)
	{
		_viewBalanceService = viewBalanceService;
	}

	[HttpGet("{userId}")]
	public async Task<IActionResult> GetBalance(string userId)
	{
		try
		{
			var balanceDto = await _viewBalanceService.GetBalanceByUserIdAsync(userId);
			if (balanceDto == null)
			{
				return NotFound($"No balance information found for user ID: {userId}.");
			}
			return Ok(balanceDto);
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
	}
}