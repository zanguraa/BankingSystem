using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.BankAccounts.CreateBankAccount
{

    public enum CurrencyType
    {
        USD = 840,  // United States Dollar
        EUR = 978,  // Euro
        GEL = 981   // Georgian Lari
    }

    public class CreateBankAccountRequest
    {
        public int UserId { get; set; }
        public string Iban { get; set; }
        public decimal InitialAmount { get; set; }
        public CurrencyType Currency { get; set; }
    }
}
