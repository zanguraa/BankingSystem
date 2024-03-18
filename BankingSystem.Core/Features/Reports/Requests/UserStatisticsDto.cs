using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Reports.Requests
{
	public class UserStatisticsDto
	{
		public int NumberOfUsersRegisteredCurrentYear { get; set; }
		public int NumberOfUsersRegisteredLastYear { get; set; }
		public int NumberOfUsersRegisteredLast30Days { get; set; }
	}
}
