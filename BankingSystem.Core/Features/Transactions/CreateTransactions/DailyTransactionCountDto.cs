using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Transactions.CreateTransactions
{
    public class DailyTransactionCountDto
    {
        public DateTime Date { get; set; }
        public int TransactionCount { get; set; }
    }
}
