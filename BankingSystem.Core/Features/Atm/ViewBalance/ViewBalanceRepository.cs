﻿using BankingSystem.Core.Data;
using BankingSystem.Core.Shared.Models;

namespace BankingSystem.Core.Features.Atm.ViewBalance;
public interface IViewBalanceRepository
{
    Task<BalanceInfo?> GetBalanceInfoByCardNumberAsync(string cardNumber);
    Task<BalanceInfo?> GetBalanceInfoByUserIdAsync(string userId);
}

public class ViewBalanceRepository : IViewBalanceRepository
{
    private readonly IDataManager _dataManager;

    public ViewBalanceRepository(IDataManager dataManager)
    {
        _dataManager = dataManager;
    }

    public async Task<BalanceInfo?> GetBalanceInfoByUserIdAsync(string userId)
    {
        var query = "SELECT UserId, InitialAmount, Currency FROM BankAccounts WHERE UserId = @UserId";
        var parameters = new { UserId = userId };
        var balanceInfo = await _dataManager.Query<BalanceInfo, dynamic>(query, parameters);
        return balanceInfo.FirstOrDefault();
    }

    public async Task<BalanceInfo?> GetBalanceInfoByCardNumberAsync(string cardNumber)
    {
        var query = @"
        SELECT b.UserId, b.InitialAmount, b.Currency 
        FROM BankAccounts b
        INNER JOIN Cards c ON b.Id = c.AccountId
        WHERE c.CardNumber = @CardNumber AND c.IsActive = 1";

        var parameters = new { CardNumber = cardNumber };
        var balanceInfo = await _dataManager.Query<BalanceInfo, dynamic>(query, parameters);

        return balanceInfo.FirstOrDefault();
    }
}
