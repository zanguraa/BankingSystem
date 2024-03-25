namespace BankingSystem.Core.Features.Atm.ChangePin.Requests
{
    public class ChangePinRequest
	{
		public string CardNumber { get; set; }
		public int CurrentPin { get; set; }
		public int NewPin { get; set; }
	}

}
