using BankingSystem.Core.Features.BankAccounts.CreateBankAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.BankAccounts
{
    public interface IBankAccountService
    {
        Task<int> CreateBankAccount(CreateBankAccountRequest createBankAccountRequest);
        Task<List<BankAccount>> GetBankAccounts();
    }
}
