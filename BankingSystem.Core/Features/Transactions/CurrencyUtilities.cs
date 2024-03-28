using BankingSystem.Core.Shared.Models;

public static class CurrencyUtilities
{
    private static readonly Dictionary<Currency, string> CurrencyCodes;

    static CurrencyUtilities()
    {
        CurrencyCodes = Enum.GetValues(typeof(Currency))
                            .Cast<Currency>()
                            .ToDictionary(k => k, v => v.ToString());
    }

    public static string GetCurrencyCode(Currency currency)
    {
        if (CurrencyCodes.TryGetValue(currency, out var code))
        {
            return code;
        }
        throw new ArgumentOutOfRangeException(nameof(currency), $"Unsupported currency: {currency}");
    }
}
