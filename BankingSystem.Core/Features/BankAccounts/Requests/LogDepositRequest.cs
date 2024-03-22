
namespace BankingSystem.Core.Features.BankAccounts.Requests;
public class LogDepositRequest
{
    public int BankAccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}
