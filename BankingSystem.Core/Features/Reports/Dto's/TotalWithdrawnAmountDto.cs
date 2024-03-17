using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Reports.Dto_s
{
    public class TotalWithdrawnAmountDto
    {
        // This dictionary will store the total withdrawn amounts for each currency.
        public Dictionary<string, decimal> TotalWithdrawnAmountsByCurrency { get; set; } = new Dictionary<string, decimal>();
    }
}
