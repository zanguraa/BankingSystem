//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BankingSystem.Core.Features.Transactions
//{
//	public class CurrencyConversionService : ICurrencyConversionService
//	{
//		public async Task<decimal> ConvertAsync(decimal amount, string fromCurrency, string toCurrency)
//		{
//			// იღებსexchange rate გარე სერვისიდან, მონაცემთა ბაზიდან და ა.შ.
//			decimal exchangeRate = await GetExchangeRateAsync(fromCurrency, toCurrency);

//			return amount * exchangeRate;
//		}

//		private async Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency)
//		{
//			if (fromCurrency == "USD" && toCurrency == "EUR")
//			{
//				return 0.85m; // 1 USD = 0.85 EUR
//			}
//			else if (fromCurrency == "EUR" && toCurrency == "USD")
//			{
//				return 1.18m; // 1 EUR = 1.18 USD (example inverse rate)
//			}
//			else if (fromCurrency == "USD" && toCurrency == "GEL")
//			{
//				return 3.10m; // 1 USD = 3.10 GEL (example rate)
//			}
//			else if (fromCurrency == "GEL" && toCurrency == "USD")
//			{
//				return 0.32m; // 1 GEL = 0.32 USD (example inverse rate)
//			}
//			else if (fromCurrency == "EUR" && toCurrency == "GEL")
//			{
//				return 3.65m; // 1 EUR = 3.65 GEL (example rate)
//			}
//			else if (fromCurrency == "GEL" && toCurrency == "EUR")
//			{
//				return 0.27m; // 1 GEL = 0.27 EUR (example inverse rate)
//			}
//			return 1; 
//		}
//	}

//	public interface ICurrencyConversionService
//	{
//		Task<decimal> ConvertAsync(decimal amount, string currency, string v);
//	}
//}
