using BankingSystem.Core.Shared.Models;

namespace BankingSystem.Core.Features.Cards.CreateCard.Models.Requests;

public class BankAccount
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Iban { get; set; }
    public decimal InitialAmount { get; set; }
    public Currency Currency { get; set; }
}