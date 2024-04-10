using BankingSystem.Core.Data;
using BankingSystem.Core.Shared.Models;

namespace BankingSystem.Core.Shared.Currency;

public interface ICurrencyConversionRepository
{
    Task<List<CurrencyModel>> GetAllCurrenciesAsync();
}

public class CurrencyConversionRepository : ICurrencyConversionRepository
{
    private readonly IDataManager _dataManager;

    public CurrencyConversionRepository(IDataManager dataManager)
    {
        _dataManager = dataManager;
    }
    public async Task<List<CurrencyModel>> GetAllCurrenciesAsync()
    {
        string query = "SELECT * FROM Currencies";

        return (await _dataManager.Query<CurrencyModel>(query)).ToList();
    }
}
