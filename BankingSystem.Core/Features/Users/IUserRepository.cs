using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Features.Users
{
    public interface IUserRepository
    {
        Task<bool> UserByPersonalIdExist(string personalId);
    }
}
