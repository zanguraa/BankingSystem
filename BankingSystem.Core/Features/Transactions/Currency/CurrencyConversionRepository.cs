using BankingSystem.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Transactions.Currency
{
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
}
