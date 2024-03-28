using BankingSystem.Core.Data;
using BankingSystem.Core.Shared.Models;

namespace BankingSystem.Core.Shared.Services.Currency;

public interface ICurrencyConversionRepository
{
    Task<List<CurrencyModel>> GetAllCurrencies();
}

public class CurrencyConversionRepository : ICurrencyConversionRepository
{
    private readonly IDataManager _dataManager;

    public CurrencyConversionRepository(IDataManager dataManager)
    {
        _dataManager = dataManager;
    }
    public async Task<List<CurrencyModel>> GetAllCurrencies()
    {
        string query = "SELECT * FROM Currencies";

        return (await _dataManager.Query<CurrencyModel>(query)).ToList();
    }
}
