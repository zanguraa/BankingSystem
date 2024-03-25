namespace BankingSystem.Core.Features.Atm.CardAuthorizations.Requests
{
    public class CardAuthorizationResponse
	{
		public bool IsAuthorized { get; set; }
		public string Message { get; set; }
	}
}
