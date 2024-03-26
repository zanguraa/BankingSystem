﻿namespace BankingSystem.Core.Features.Transactions.CreateTransactions
{
    public class TransactionResponse
    {
        public int? FromAccountId { get; set; }
        public int? ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public decimal Fee { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
