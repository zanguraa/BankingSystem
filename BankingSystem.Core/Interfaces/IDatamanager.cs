using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Interfaces
{
    public interface IDatamanager
    {
         int Execute<T>(string sql, T item);
         IEnumerable<T> Query<T, P>(string sql, P parameters);
    }
}
