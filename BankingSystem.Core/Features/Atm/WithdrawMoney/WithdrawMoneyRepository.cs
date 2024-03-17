using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Atm.WithdrawMoney.Requests;

namespace BankingSystem.Core.Features.Atm.WithdrawMoney
{
    public class WithdrawMoneyRepository : IWithdrawMoneyRepository
    {
        private readonly IDataManager _dataManager;

        public WithdrawMoneyRepository(IDataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public Task<bool> RecordTransactionAsync(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> WithdrawAsync(WithdrawRequest request)
        {
            var transactionCommands = new List<SqlCommandRequest>
            {
                 new SqlCommandRequest
            {
                Query = @"
                    UPDATE BankAccounts
                    SET InitialAmount = InitialAmount - @Amount
                    WHERE Id = @AccountId",
                Params = new { request.AccountId, request.Amount }
            },
                new SqlCommandRequest
            {
            Query = @"
                INSERT INTO DailyWithdrawals 
                (BankAccountId, WithdrawalDate, TotalAmount, Currency, RequestedAmount, RequestedCurrency)
                VALUES (@BankAccountId, GETDATE(), @TotalAmount, @Currency, @RequestedAmount, @RequestedCurrency)",
            // Ensure you pass the correct parameters for RequestedAmount and RequestedCurrency
            // It might involve capturing these from the request or calculating them as necessary before this method is called.
                Params = new
            {
                BankAccountId = request.AccountId,
                TotalAmount = request.Amount, // This might need adjustment if it refers to the converted amount
                Currency = request.Currency, // This might need adjustment if it refers to the converted currency
                RequestedAmount = request.RequestedAmount, // Assuming these are new properties or calculated values you have
                RequestedCurrency = request.RequestedCurrency
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

