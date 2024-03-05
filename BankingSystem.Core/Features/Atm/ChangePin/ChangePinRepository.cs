//using System.Threading.Tasks;
//using BankingSystem.Core.Data;
//using BankingSystem.Core.Features.Atm.ChangePin;
//using BankingSystem.Core.Features.Cards;

//public class ChangePinRepository : IChangePinRepository
//{
//	private readonly IDataManager _dataManager;

//	public ChangePinRepository(IDataManager dataManager)
//	{
//		_dataManager = dataManager;
//	}

//	public async Task<Card> GetCardByNumberAsync(string cardNumber)
//	{
//		var query = "SELECT * FROM Cards WHERE CardNumber = @CardNumber";
//		var card = await _dataManager.Query<Card, dynamic>(query, new { CardNumber = cardNumber });
//		return card.First();
//	}

//	public async Task<bool> UpdatePinAsync(string cardNumber, string currentPin, string newPin)
//	{
//		var updatePinQuery = "UPDATE Cards SET Pin = @NewPin WHERE CardNumber = @CardNumber AND Pin = @CurrentPin";
//		var result = await _dataManager.Execute(updatePinQuery, new { CardNumber = cardNumber, CurrentPin = currentPin, NewPin = newPin });
//		return result > 0;
//	}
//}