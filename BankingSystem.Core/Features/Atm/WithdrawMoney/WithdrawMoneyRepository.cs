using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Atm.WithdrawMoney.Models;
using BankingSystem.Core.Features.Atm.WithdrawMoney.Models.Requests;
using BankingSystem.Core.Shared.Exceptions;
using BankingSystem.Core.Shared.Models;

namespace BankingSystem.Core.Features.Atm.WithdrawMoney;

public interface IWithdrawMoneyRepository
{
    Task<DecimalSum?> GetWithdrawalsOf24hoursByCardIdAsync(WithdrawalCheck options);
    Task<BalanceInfo?> GetBalanceInfoByCardNumberAsync(string cardNumber);
    Task<Card> GetCardByNumberAsync(string CardNumber);
    Task<bool> ProcessAtmTransaction(Transaction transactionRequest);
}

public class WithdrawMoneyRepository : IWithdrawMoneyRepository
{
    private readonly IDataManager _dataManager;

    public WithdrawMoneyRepository(IDataManager dataManager)
    {
        _dataManager = dataManager;
    }

    public async Task<DecimalSum?> GetWithdrawalsOf24hoursByCardIdAsync(WithdrawalCheck options)
    {
        var query = @"SELECT SUM(d.TotalAmount * c.Rate) AS Sum FROM DailyWithdrawals AS d
                          INNER JOIN Currencies AS c ON d.Currency = c.Code
                          WHERE d.BankAccountId = @BankAccountId AND WithdrawalDate >= @WithdrawalDate";

        var result = await _dataManager.Query<DecimalSum, dynamic>(query, options);
        return
            result.FirstOrDefault();
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

    public async Task<Card> GetCardByNumberAsync(string CardNumber)
    {
        var query = "SELECT * FROM Cards WHERE CardNumber = @CardNumber";
        var result = await _dataManager.Query<Card, dynamic>(query, new { CardNumber })
            ?? throw new InvalidCardException("card not found {CardNumber}", CardNumber);

        return result.FirstOrDefault();
    }

    public async Task<bool> ProcessAtmTransaction(Transaction transactionRequest)
    {
        var SqlCommandList = new List<SqlCommand>
             {
                new() {
                    Query = @"
                        UPDATE BankAccounts
                        SET InitialAmount = InitialAmount - @Amount 
                        WHERE Id = @AccountId",
                    Params = new { AccountId = transactionRequest.FromAccountId, Amount = transactionRequest.FromAmount + transactionRequest.Fee}
                },
                new() {
                    Query = @"
                        INSERT INTO Transactions (FromAccountId, ToAccountId, FromAccountCurrency, ToAccountCurrency, FromAmount, ToAmount, TransactionDate, TransactionType, Fee)
                        VALUES (@FromAccountId, @ToAccountId, @FromAccountCurrency, @ToAccountCurrency, @FromAmount, @ToAmount, @TransactionDate, @TransactionType, @Fee);",
                    Params = transactionRequest
                }
            };
        bool success = await _dataManager.ExecuteWithTransaction(SqlCommandList);

        if (!success) throw new Exception("An error occurred while processing your request.");

        return success;
    }
}

