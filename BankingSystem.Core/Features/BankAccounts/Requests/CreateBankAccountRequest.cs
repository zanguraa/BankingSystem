using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.BankAccounts.CreateBankAccount
{

    public class CreateBankAccountRequest
    {
        public int UserId { get; set; }
        public string Iban { get; set; }
    }
}
