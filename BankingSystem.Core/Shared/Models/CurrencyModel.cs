namespace BankingSystem.Core.Shared.Models;

public class CurrencyModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public decimal Rate { get; set; }
}
