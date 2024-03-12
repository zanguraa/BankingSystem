using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BankingSystem.Core.Features.Reports;
using BankingSystem.Core.Features.Reports.Dto_s;

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

	[HttpGet("total-withdrawn-amount")]
	public async Task<IActionResult> GetTotalWithdrawnAmount([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
	{
		var result = await _reportsService.GetTotalWithdrawnAmountAsync(startDate, endDate);
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