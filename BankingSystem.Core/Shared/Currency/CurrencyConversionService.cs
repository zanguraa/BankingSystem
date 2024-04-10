namespace BankingSystem.Core.Shared.Currency;

public interface ICurrencyConversionService
{
    decimal Convert(decimal amount, string fromCurrency, string toCurrency);
}

public class CurrencyConversionService : ICurrencyConversionService
{
    private readonly ICurrencyConversionRepository _currencyConversionRepository;
    private readonly Dictionary<string, decimal> _currencyPairs = new();

    public CurrencyConversionService(ICurrencyConversionRepository currencyConversionRepository)
    {
        _currencyConversionRepository = currencyConversionRepository;
        LoadCurrencyPairsAsync().Wait();
    }

    private async Task LoadCurrencyPairsAsync()
    {
        var currencies = await _currencyConversionRepository.GetAllCurrenciesAsync();
        foreach (var currency in currencies)
        {
            _currencyPairs.Add(currency.Code, currency.Rate);
        }
    }

    public decimal Convert(decimal amount, string fromCurrency, string toCurrency)
    {
        ValidateCurrency(fromCurrency);
        ValidateCurrency(toCurrency);

        decimal amountInGEL = amount * GetRateToGEL(fromCurrency);

        return ConvertFromGEL(amountInGEL, toCurrency);
    }

    private void ValidateCurrency(string currency)
    {
        if (!_currencyPairs.ContainsKey(currency))
        {
            throw new ArgumentException($"Unsupported currency: {currency}");
        }
    }

    private decimal GetRateToGEL(string currency)
    {
        // Directly return the rate to convert to GEL
        return _currencyPairs[currency];
    }

    private decimal ConvertFromGEL(decimal amount, string toCurrency)
    {
        // If converting to GEL, rate is 1, otherwise, use the inverse of the rate from GEL
        decimal rateFromGEL = toCurrency == "GEL" ? 1 : 1 / _currencyPairs[toCurrency];
        return amount * rateFromGEL;
    }
}
