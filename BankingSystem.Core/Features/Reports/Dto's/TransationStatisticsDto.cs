using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Reports.Dto_s
{
	public class TransactionStatisticsDto
	{
		public int TransactionsLastMonth { get; set; }
		public int TransactionsLastSixMonths { get; set; }
		public int TransactionsLastYear { get; set; }

		// Income received from transactions
		public decimal IncomeLastMonthGEL { get; set; }
		public decimal IncomeLastSixMonthsGEL { get; set; }
		public decimal IncomeLastYearGEL { get; set; }
		public decimal IncomeLastMonthUSD { get; set; }
		public decimal IncomeLastSixMonthsUSD { get; set; }
		public decimal IncomeLastYearUSD { get; set; }
		public decimal IncomeLastMonthEUR { get; set; }
		public decimal IncomeLastSixMonthsEUR { get; set; }
		public decimal IncomeLastYearEUR { get; set; }

		// Average revenue from one transaction
		public decimal AverageRevenuePerTransactionGEL { get; set; }
		public decimal AverageRevenuePerTransactionUSD { get; set; }
		public decimal AverageRevenuePerTransactionEUR { get; set; }
	}
