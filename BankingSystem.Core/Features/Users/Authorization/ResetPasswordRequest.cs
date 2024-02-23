using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Users.Authorization
{
    public class ResetPasswordRequest
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
