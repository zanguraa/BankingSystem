internal class TransactionLog
{
    public int RequestedAmount { get; set; }
    public string RequestedCurrency { get; set; }
    public decimal DeductedAmount { get; set; }
    public string AccountCurrency { get; set; }
    public int BankAccountId { get; set; }
    public DateTime WithdrawalDate { get; set; }
}