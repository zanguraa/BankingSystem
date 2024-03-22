using BankingSystem.Core.Data;

namespace BankingSystem.Core.Features.Users
{
    public class UserRepository : IUserRepository
    {

        private readonly IDataManager _dataManager;
        public UserRepository(IDataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public async Task<bool> UserByPersonalIdExist(string personalId)
        {
            string sql = "SELECT TOP 1 1 FROM Users WHERE PersonalId = @PersonalId";

            var result = await _dataManager.Query<int, dynamic>(
                sql,
                new { PersonalId = personalId }
            );

            return result.Any();
        }

    }
}
