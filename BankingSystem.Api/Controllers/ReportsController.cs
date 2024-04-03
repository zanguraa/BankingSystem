using BankingSystem.Core.Features.Reports.Shared.Requests;
using BankingSystem.Core.Features.Reports.TransactionStatistics;
using BankingSystem.Core.Features.Reports.UserStatistics;
using BankingSystem.Core.Features.Reports.Withdrawals;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly ITransactionStatisticsService _transactionStatisticsService;
    private readonly IWithdrawalsService _withdrawalsService;
    private readonly IUserStatisticsService _userStatisticsService;

    public ReportsController(
        IWithdrawalsService withdrawalsService,
        IUserStatisticsService userStatisticsService,
        ITransactionStatisticsService transactionStatisticsService)
    {
        _withdrawalsService = withdrawalsService;
        _userStatisticsService = userStatisticsService;
        _transactionStatisticsService = transactionStatisticsService;
    }

    [HttpPost("transaction-statistics")]
    public async Task<IActionResult> GetTransactionStatistics([FromBody] ReportsRequest request)
    {
        var result = await _transactionStatisticsService.GetTransactionStatisticsAsync(request);
        return Ok(result);
    }

    [HttpPost("daily-transaction-counts")]
    public async Task<IActionResult> GetDailyTransactionCounts([FromBody] ReportsRequest request)
    {
        var result = await _transactionStatisticsService.GetDailyTransactionCountsAsync(request);
        return Ok(result);
    }

    [HttpPost("total-withdrawn-amount")]
    public async Task<IActionResult> GetTotalWithdrawnAmount([FromBody] ReportsRequest request)
    {
        var result = await _withdrawalsService.GetTotalWithdrawnAmountAsync(request);
        return Ok(result);
    }

    [HttpPost("average-revenue-per-transaction")]
    public async Task<IActionResult> GetAverageRevenuePerTransaction([FromBody] ReportsRequest request)
    {
        var result = await _transactionStatisticsService.GetAverageRevenuePerTransactionAsync(request);
        return Ok(result);
    }

    [HttpGet("user-statistics")]
    public async Task<IActionResult> GetUserStatistics()
    {
        var result = await _userStatisticsService.GetUserStatisticsAsync();
        return Ok(result);
    }
}