namespace BankingSystem.Core.Shared.Models;
public class BankAccount
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Iban { get; set; }
    public decimal InitialAmount { get; set; }
    public Currency Currency { get; set; }
}