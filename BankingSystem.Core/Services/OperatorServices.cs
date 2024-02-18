using BankingSystem.Core.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Services
{
    public class OperatorServices
    {
        public bool RegisterOperator(CreateOperatorRequest request)
        {
            if(request == null)
            {
                return false;
            }
            if (!string.IsNullOrEmpty(request.OperatorId) && !int.TryParse(request.OperatorId, out _))
            {
                return false;
            }
            if (string.IsNullOrEmpty(request.Username))
            {
                return false;
            }
            if (string.IsNullOrEmpty(request.Password))
            {
                return false;
            }



            return true;
        }
    }
}
