namespace BankingSystem.Core.Features.Transactions
{
	public interface ICurrencyConversionService
	{
		Task<decimal> ConvertAsync(decimal amount, string currency, string v);
	}
}
