namespace BankingSystem.Core.Features.Transactions.Currency
{
    public interface ICurrencyConversionService
    {
        Task<decimal> ConvertAsync(decimal amount, string currency, string v);
    }
}
