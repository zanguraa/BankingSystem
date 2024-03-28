namespace BankingSystem.Core.Features.Atm.CardAuthorizations.Models.Response
{
    public class CardAuthorizationResponse
    {
        public bool IsAuthorized { get; set; }
        public bool IsActive { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
    }
}
