using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Atm.ViewBalance;
using Dapper;
using System.Data;
using System.Threading.Tasks;

public class ViewBalanceRepository : IViewBalanceRepository
{
	private readonly IDataManager _dataManager;

	public ViewBalanceRepository(IDataManager dataManager)
	{
		_dataManager = dataManager;
	}

	public async Task<BalanceInfo?> GetBalanceUserIdAsync(string userId)
	{
		var query = "SELECT UserId, InitialAmount, Currency FROM BankAccounts WHERE UserId = @UserId";

		// Using Dapper to execute the query
		var balanceInfo = await _dataManager.Query<BalanceInfo,dynamic>(query, new { UserId = userId });

		return balanceInfo.FirstOrDefault();
	}
	public async Task<BalanceInfo?> GetBalanceInfoByUserIdAsync(string userId)
	{
		var query = "SELECT UserId, InitialAmount, Currency FROM BankAccounts WHERE UserId = @UserId";
		var parameters = new { UserId = userId };
		var balanceInfo = await _dataManager.Query<BalanceInfo,dynamic>(query, parameters);
		return balanceInfo.FirstOrDefault();
	}
	public Task<BalanceInfo> GetBalanceInfoAsync(string identifier)
	{
		throw new NotImplementedException();
	}
}
