using BankingSystem.Core.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Interfaces
{
    public interface IOperatorServices
    {
        Task<bool> RegisterOperator(CreateOperatorRequest request);
    }
}
