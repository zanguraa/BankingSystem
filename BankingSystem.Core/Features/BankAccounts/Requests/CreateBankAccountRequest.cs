namespace BankingSystem.Core.Features.BankAccounts.Requests;

public class CreateBankAccountRequest
{
    public int UserId { get; set; }
    public string Iban { get; set; }
}
