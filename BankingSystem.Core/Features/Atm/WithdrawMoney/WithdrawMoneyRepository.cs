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

