using Azure.Core;
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

	[HttpPost("transaction-statistics")]
	public async Task<IActionResult> GetTransactionStatistics([FromBody] ReportsRequest request)
	{
		var result = await _reportsService.GetTransactionStatisticsAsync(request);
		return Ok(result);
	}

	[HttpPost("daily-transaction-counts")]
	public async Task<IActionResult> GetDailyTransactionCounts([FromBody] ReportsRequest request)
	{
		var result = await _reportsService.GetDailyTransactionCountsAsync(request);
		return Ok(result);
	}

	[HttpPost("total-withdrawn-amount")]
	public async Task<IActionResult> GetTotalWithdrawnAmount([FromBody] ReportsRequest request)
	{
		var result = await _reportsService.GetTotalWithdrawnAmountAsync(request);
		return Ok(result);
	}

	[HttpPost("average-revenue-per-transaction")]
	public async Task<IActionResult> GetAverageRevenuePerTransaction([FromBody] ReportsRequest request)
	{
		var result = await _reportsService.GetAverageRevenuePerTransactionAsync(request);
		return Ok(result);
	}

	[HttpGet("user-statistics")]
	public async Task<IActionResult> GetUserStatistics()
	{
		var result = await _reportsService.GetUserStatisticsAsync();
		return Ok(result);
	}
}