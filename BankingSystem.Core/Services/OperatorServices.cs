using BankingSystem.Core.Interfaces;
using BankingSystem.Core.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Services
{
    public class OperatorServices : IOperatorServices
    {
        private readonly IOperatorRepository _operatorRepository;

        public OperatorServices( IOperatorRepository operatorRepository)
        {
            _operatorRepository = operatorRepository;
        }
        

        public bool RegisterOperator(CreateOperatorRequest request)
        {
            if(request == null)
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

            var data = _operatorRepository.AddOperator(request);

            return true;
        }
    }
}
