using System.Text.Json.Serialization;

namespace BankingSystem.Core.Features.Atm.ChangePin.Models.Requests
{
    public class ChangePinRequest
    {
        public int CurrentPin { get; set; }
        public int NewPin { get; set; }
        [JsonIgnore]
        public string? CardNumber { get; set; }

    }

}
