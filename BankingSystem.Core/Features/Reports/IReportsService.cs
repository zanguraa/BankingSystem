﻿using BankingSystem.Core.Features.Reports.Requests;
using Microsoft.AspNetCore.Mvc;

public interface IReportsService
{
    Task<TransactionStatisticsDto> GetTransactionStatisticsAsync(DateTime startDate, DateTime endDate);
    Task<Dictionary<string, int>> GetDailyTransactionCountsAsync(DateTime startDate, DateTime endDate);
    Task<TotalWithdrawnAmountDto> GetTotalWithdrawnAmountAsync(DateTime startDate, DateTime endDate);
    Task<TransactionStatisticsDto> GetAverageRevenuePerTransactionAsync(DateTime startDate, DateTime endDate);
    Task<UserStatisticsDto> GetUserStatisticsAsync();
}