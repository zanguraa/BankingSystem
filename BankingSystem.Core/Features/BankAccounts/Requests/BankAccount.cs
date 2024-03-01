using BankingSystem.Core.Features.BankAccounts;

public class BankAccount
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public string Iban { get; set; }
    public decimal InitialAmount { get; set; }
    public CurrencyType Currency { get; set; }
}