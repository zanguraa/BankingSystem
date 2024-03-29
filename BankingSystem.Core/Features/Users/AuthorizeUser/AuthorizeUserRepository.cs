using BankingSystem.Core.Data;

namespace BankingSystem.Core.Features.Users.AuthorizeUser;

public interface IAuthorizeUserRepository
{
    Task<bool> UserExistsAsync(int userId);
}

public class AuthorizeUserRepository : IAuthorizeUserRepository
{
    private readonly IDataManager _dataManager;
    public AuthorizeUserRepository(IDataManager dataManager)
    {
        _dataManager = dataManager;
    }


    public async Task<bool> UserExistsAsync(int userId)
    {
        string sql = "SELECT TOP 1 1 FROM [BankingSystem_db].[dbo].[Users] WHERE id = @userId";

        var result = await _dataManager.Query<int, dynamic>(sql, new { userId });
        return result.Any();
    }
}
