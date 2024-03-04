using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Transactions.Currency
{
    public class CurrencyConversionService : ICurrencyConversionService
    {
        private readonly I

        private readonly Dictionary<string, decimal> _baseRatesToGEL = new Dictionary<string, decimal>
        {
            {"USD", 2.65m}, // 1 USD = 2.65 GEL
            {"EUR", 2.85m}, // 1 EUR = 2.85 GEL
            {"GEL", 1m}     // Base rate for GEL to GEL
        };

        public decimal Convert(decimal amount, string fromCurrency, string toCurrency)
        {
            // Ensure both currencies are supported
            ValidateCurrency(fromCurrency);
            ValidateCurrency(toCurrency);

            // Convert from the original currency to GEL
            decimal amountInGEL = amount * GetRateToGEL(fromCurrency);

            // Convert from GEL to the target currency
            return ConvertFromGEL(amountInGEL, toCurrency);
        }

        private void ValidateCurrency(string currency)
        {
            if (!_baseRatesToGEL.ContainsKey(currency))
            {
                throw new ArgumentException($"Unsupported currency: {currency}");
            }
        }

        private decimal GetRateToGEL(string currency)
        {
            // Directly return the rate to convert to GEL
            return _baseRatesToGEL[currency];
        }

        private decimal ConvertFromGEL(decimal amount, string toCurrency)
        {
            // If converting to GEL, rate is 1, otherwise, use the inverse of the rate from GEL
            decimal rateFromGEL = toCurrency == "GEL" ? 1 : 1 / _baseRatesToGEL[toCurrency];
            return amount * rateFromGEL;
        }
    }
}
