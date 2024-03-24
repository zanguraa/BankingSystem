using BankingSystem.Core.Features.BankAccounts.Requests;

public static class CurrencyUtilities
{
    private static readonly Dictionary<CurrencyType, string> CurrencyCodes;

    static CurrencyUtilities()
    {
        CurrencyCodes = Enum.GetValues(typeof(CurrencyType))
                            .Cast<CurrencyType>()
                            .ToDictionary(k => k, v => v.ToString());
    }

    public static string GetCurrencyCode(CurrencyType currency)
    {
        if (CurrencyCodes.TryGetValue(currency, out var code))
        {
            return code;
        }
        throw new ArgumentOutOfRangeException(nameof(currency), $"Unsupported currency: {currency}");
    }
}
