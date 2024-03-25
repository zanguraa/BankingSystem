namespace BankingSystem.Core.Features.Reports.Requests
{
    public class UserStatisticsDto
	{
		public int NumberOfUsersRegisteredCurrentYear { get; set; }
		public int NumberOfUsersRegisteredLastYear { get; set; }
		public int NumberOfUsersRegisteredLast30Days { get; set; }
	}
}
