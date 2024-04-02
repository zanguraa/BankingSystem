using System.Text.Json.Serialization;

namespace BankingSystem.Core.Features.Atm.ChangePin.Models.Requests
{
    public class ChangePinRequest
    {
        public string CurrentPin { get; set; }
        public string NewPin { get; set; }
        [JsonIgnore]
        public string? CardNumber { get; set; }

    }

}
