﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Atm.WithdrawMoney.Dto_s
{
	public class WithdrawRequestDto
	{
		public int AccountId { get; set; }
		public decimal Amount { get; set; }
		public string Currency { get; set; }
	}
}
