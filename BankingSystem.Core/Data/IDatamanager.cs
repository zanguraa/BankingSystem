using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Data
{
    public interface IDataManager
    {
        Task<int> Execute<T>(string sql, T item);
        Task<IEnumerable<T>> Query<T, P>(string sql, P parameters);
        Task<IEnumerable<T>> Query<T>(string sql);
        Task<bool> ExecuteWithTransaction(List<SqlCommandRequest> dataRequest);
    }
}
