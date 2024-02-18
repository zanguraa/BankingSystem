using BankingSystem.Core.Models.Requests;

namespace BankingSystem.Core.Interfaces
{
    public interface IOperatorRepository
    {
        bool AddOperator(CreateOperatorRequest request);
        bool OperatorExists(string username);
    }
}