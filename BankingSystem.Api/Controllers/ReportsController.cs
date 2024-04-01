using BankingSystem.Core.Features.Reports;
using BankingSystem.Core.Features.Reports.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
	private readonly IReportsService _reportsService;

	public ReportsController(IReportsService reportsService)
	{
		_reportsService = reportsService;
	}

	[HttpGet("transaction-statistics")]
	public async Task<IActionResult> GetTransactionStatistics([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
	{
		var result = await _reportsService.GetTransactionStatisticsAsync(startDate, endDate);
		return Ok(result);
	}

	[HttpGet("daily-transaction-counts")]
	public async Task<IActionResult> GetDailyTransactionCounts([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
	{
		var result = await _reportsService.GetDailyTransactionCountsAsync(startDate, endDate);
		return Ok(result);
	}

	[HttpPost("total-withdrawn-amount")]
	public async Task<IActionResult> GetTotalWithdrawnAmount([FromBody] ReportsRequest request)
	{
		var result = await _reportsService.GetTotalWithdrawnAmountAsync(request);
		return Ok(result);
	}

	[HttpGet("average-revenue-per-transaction")]
	public async Task<IActionResult> GetAverageRevenuePerTransaction([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
	{
		var result = await _reportsService.GetAverageRevenuePerTransactionAsync(startDate, endDate);
		return Ok(result);
	}

	[HttpGet("user-statistics")]
	public async Task<IActionResult> GetUserStatistics()
	{
		var result = await _reportsService.GetUserStatisticsAsync();
		return Ok(result);
	}
}