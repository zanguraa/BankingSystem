using BankingSystem.Core.Data;
using BankingSystem.Core.Features.Reports.Shared.Requests;

namespace BankingSystem.Core.Features.Reports.UserStatistics;

public interface IUserStatisticsRepository
{
    Task<UserStatisticsDto> GetUserStatisticsAsync();
}

public class UserStatisticsRepository : IUserStatisticsRepository
{
    public readonly IDataManager _dataManager;
    public UserStatisticsRepository(IDataManager dataManager)
    {
        _dataManager = dataManager;
    }

    public async Task<UserStatisticsDto> GetUserStatisticsAsync()
    {
        var currentYearQuery = @"
                SELECT COUNT(*) 
                FROM Users 
                WHERE YEAR(RegisterDate) = YEAR(GETDATE());";

        var lastYearQuery = @"
                SELECT COUNT(*) 
                FROM Users 
                WHERE YEAR(RegisterDate) = YEAR(GETDATE()) - 1;";

        var last30DaysQuery = @"
                SELECT COUNT(*) 
                FROM Users 
                WHERE RegisterDate >= DATEADD(DAY, -30, GETDATE());";

        var currentYearCount = (await _dataManager.Query<int>(currentYearQuery)).FirstOrDefault();
        var lastYearCount = (await _dataManager.Query<int>(lastYearQuery)).FirstOrDefault();
        var last30DaysCount = (await _dataManager.Query<int>(last30DaysQuery)).FirstOrDefault();

        return new UserStatisticsDto
        {
            NumberOfUsersRegisteredCurrentYear = currentYearCount,
            NumberOfUsersRegisteredLastYear = lastYearCount,
            NumberOfUsersRegisteredLast30Days = last30DaysCount
        };
    }
}
