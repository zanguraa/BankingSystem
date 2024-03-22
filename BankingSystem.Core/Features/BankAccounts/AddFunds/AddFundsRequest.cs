using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.BankAccounts.AddFunds;

public class AddFundsRequest
{
    public int BankAccountId { get; set; }
    public decimal Amount { get; set; }
}

