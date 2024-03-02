namespace BankingSystem.Core.Features.Transactions.Currency
{
    public interface ICurrencyConversionService
    {
        decimal Convert(decimal amount, string fromCurrency, string toCurrency);
    }
}
