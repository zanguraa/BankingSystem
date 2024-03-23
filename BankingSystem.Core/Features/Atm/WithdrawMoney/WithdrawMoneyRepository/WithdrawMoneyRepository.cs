﻿using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Atm.WithdrawMoney.Requests;

namespace BankingSystem.Core.Features.Atm.WithdrawMoney.WithdrawMoneyRepository
{
    public class WithdrawMoneyRepository : IWithdrawMoneyRepository
    {
        private readonly IDataManager _dataManager;

        public WithdrawMoneyRepository(IDataManager dataManager)
        {
            _dataManager = dataManager;
        }



        public async Task<bool> WithdrawAsync(WithdrawRequest request)
        {
            var transactionCommands = new List<SqlCommand>
            {
                 new SqlCommand
            {
                Query = @"
                    UPDATE BankAccounts
                    SET InitialAmount = InitialAmount - @Amount
                    WHERE Id = @AccountId",
                Params = new { request.AccountId, request.Amount }
            },
                new SqlCommand
            {
            Query = @"
                INSERT INTO DailyWithdrawals 
                (BankAccountId, WithdrawalDate, TotalAmount, Currency, RequestedAmount, RequestedCurrency)
                VALUES (@BankAccountId, GETDATE(), @TotalAmount, @Currency, @RequestedAmount, @RequestedCurrency)",


                Params = new
            {
                BankAccountId = request.AccountId,
                TotalAmount = request.Amount,
                request.Currency,
                request.RequestedAmount,
                request.RequestedCurrency
            }
        }
    };

            return await _dataManager.ExecuteWithTransaction(transactionCommands);
        }


        public async Task<DecimalSum?> GetWithdrawalsOf24hoursByCardId(WithdrawalCheck options)
        {
            var query = @"SELECT SUM(d.TotalAmount * c.Rate) AS Sum FROM DailyWithdrawals AS d
                          INNER JOIN Currencies AS c ON d.Currency = c.Code
                          WHERE d.BankAccountId = @BankAccountId AND WithdrawalDate >= @WithdrawalDate";

            var result = await _dataManager.Query<DecimalSum, dynamic>(query, options);
            return
                result.FirstOrDefault();
        }

        public Task<bool> WithdrawAsync(string accountNumber, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}

