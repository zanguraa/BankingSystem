using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BankingSystem.Core.Features.Transactions;
using BankingSystem.Core.Features.Atm.WithdrawMoney.Dto_s;
using BankingSystem.Core.Features.Atm.WithdrawMoney;

[Route("api/[controller]")]
[ApiController]
public class WithdrawMoneyController : ControllerBase
{
	private readonly IWithdrawMoneyService _withdrawMoneyService;

	public WithdrawMoneyController(IWithdrawMoneyService withdrawMoneyService)
	{
		_withdrawMoneyService = withdrawMoneyService;
	}

	[HttpPost]
	public async Task<IActionResult> Withdraw([FromBody] WithdrawRequestDto requestDto)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		try
		{
			var responseDto = await _withdrawMoneyService.WithdrawAsync(requestDto);
			if (responseDto.IsSuccessful)
			{
				return Ok(responseDto);
			}
			else
			{
				return BadRequest(responseDto);
			}
		}
		catch (Exception ex)
		{
			return StatusCode(500, "An internal server error has occurred.");
		}
	}
}