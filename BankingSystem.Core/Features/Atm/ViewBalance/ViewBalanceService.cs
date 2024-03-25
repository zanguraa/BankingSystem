﻿using BankingSystem.Core.Features.Atm.ViewBalance.Requests;

namespace BankingSystem.Core.Features.Atm.ViewBalance;

public interface IViewBalanceService
{
    Task<BalanceResponse> GetBalanceByCardNumberAsync(string cardNumber);
    Task<BalanceResponse> GetBalanceByUserIdAsync(string userId);
}

public class ViewBalanceService : IViewBalanceService
{
    private readonly IViewBalanceRepository _viewBalanceRepository;

    public ViewBalanceService(IViewBalanceRepository viewBalanceRepository)
    {
        _viewBalanceRepository = viewBalanceRepository;
    }

    public async Task<BalanceResponse> GetBalanceByUserIdAsync(string userId)
    {
        var balanceInfo = await _viewBalanceRepository.GetBalanceInfoByUserIdAsync(userId);

        if (balanceInfo == null)
        {
            throw new KeyNotFoundException($"No balance information found for user ID: {userId}.");
        }

        return new BalanceResponse
        {
            UserId = balanceInfo.UserId,
            InitialAmount = balanceInfo.InitialAmount,
            Currency = balanceInfo.Currency
        };
    }

    public async Task<BalanceResponse> GetBalanceByCardNumberAsync(string cardNumber)
    {
        var balanceInfo = await _viewBalanceRepository.GetBalanceInfoByCardNumberAsync(cardNumber);

        if (balanceInfo == null)
        {
            throw new KeyNotFoundException($"No balance information found for card number: {cardNumber}.");
        }

        return new BalanceResponse
        {
            UserId = balanceInfo.UserId,
            InitialAmount = balanceInfo.InitialAmount,
            Currency = balanceInfo.Currency
        };
    }

}
