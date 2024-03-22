namespace BankingSystem.Core.Features.BankAccounts.AddFunds;

public class AddFundsRequest
{
    public int BankAccountId { get; set; }
    public decimal Amount { get; set; }
}

