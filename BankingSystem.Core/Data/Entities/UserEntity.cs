using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Data.Entities
{
    public class UserEntity : IdentityUser<int>
    {
        public DateTime BirthdayDate { get; set; }
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public string PersonalId { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
