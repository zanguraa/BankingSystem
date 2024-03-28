using System.Text.Json.Serialization;

namespace BankingSystem.Core.Features.Atm.WithdrawMoney.Requests
{
    public class WithdrawAmountCurrencyRequest
    {
        public string Currency { get; set; }
        public int Amount { get; set; }
        [JsonIgnore]
        public string? CardNumber { get; set; }
    }
}
