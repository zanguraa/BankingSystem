namespace BankingSystem.Core.Features.Transactions.Currency
{
    public interface ICurrencyConversionRepository
    {
        Task<List<CurrencyModel>> GetAllCurrencies();
    }
}
