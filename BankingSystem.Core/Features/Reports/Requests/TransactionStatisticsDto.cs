namespace BankingSystem.Core.Features.Reports.Requests
{
    public class TransactionStatisticsDto
	{
        public int TransactionsCount { get; set; } // ტრანზაქციების საერთო რაოდენობა
        public decimal IncomeGEL { get; set; } // შემოსავალი ლარში
        public decimal IncomeUSD { get; set; } // შემოსავალი აშშ დოლარში
        public decimal IncomeEUR { get; set; } // შემოსავალი ევროში

    }
}

