﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Reports.Dto_s
{
    public class TotalWithdrawnAmountDto
    {
        public Dictionary<string, decimal> TotalWithdrawnAmountsByCurrency { get; set; } = new Dictionary<string, decimal>();
    }
}
