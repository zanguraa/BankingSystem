using BankingSystem.Core.Data;

namespace BankingSystem.Core.Features.Users
{
    public interface ICreateUserRepository
    {
        Task<bool> UserByPersonalIdExist(string personalId);
    }

    public class CreateUserRepository : ICreateUserRepository
    {

        private readonly IDataManager _dataManager;
        public CreateUserRepository(IDataManager dataManager)
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
