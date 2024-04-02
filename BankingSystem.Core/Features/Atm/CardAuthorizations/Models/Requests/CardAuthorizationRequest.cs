namespace BankingSystem.Core.Features.Atm.CardAuthorizations.Models.Requests
{
    public class CardAuthorizationRequest
    {
        public string? CardNumber { get; set; }
        public string Pin { get; set; }

    }
}
