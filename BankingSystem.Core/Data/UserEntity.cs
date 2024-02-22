using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Data
{
	public class UserEntity : IdentityUser<int>
	{
		public DateTime BirthdayDate { get; set; }
		public string PersonalId { get; set; }
		public string PhoneNumber { get; set; }
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PersonalNumber { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public virtual ICollection<AccountEntity> Accounts { get; set; }

	}
}
