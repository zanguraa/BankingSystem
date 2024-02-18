using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Interfaces
{
    public interface IDatamanager
    {
        public int Execute<T>(string sql, T item);
    }
}
