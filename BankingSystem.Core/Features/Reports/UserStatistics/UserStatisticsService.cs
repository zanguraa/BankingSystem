using BankingSystem.Core.Features.Reports.Shared.Requests;

namespace BankingSystem.Core.Features.Reports.UserStatistics
{
    public interface IUserStatisticsService
    {
        Task<UserStatisticsDto> GetUserStatisticsAsync();
    }

    public class UserStatisticsService : IUserStatisticsService
    {
        private readonly IUserStatisticsRepository _userStatisticsRepository;
        public UserStatisticsService(IUserStatisticsRepository userStatisticsRepository)
        {
            _userStatisticsRepository = userStatisticsRepository;
        }
        public async Task<UserStatisticsDto> GetUserStatisticsAsync()
        {
            return await _userStatisticsRepository.GetUserStatisticsAsync();
        }
    }

}
