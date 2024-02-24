using BankingSystem.Core.Features.BankAccounts.CreateBankAccount;

public class BankAccount
{
    public enum CurrencyType
    {
        USD = 840,  // United States Dollar
        EUR = 978,  // Euro
        GEL = 981   // Georgian Lari
    }

    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Iban { get; set; }
    public decimal InitialAmount { get; set; }
    public CurrencyType Currency { get; set; }
}