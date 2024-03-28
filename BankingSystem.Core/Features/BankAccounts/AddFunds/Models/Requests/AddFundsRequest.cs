namespace BankingSystem.Core.Features.BankAccounts.AddFunds.Models.Requests;

public class AddFundsRequest
{
    public int BankAccountId { get; set; }
    public decimal Amount { get; set; }
}

