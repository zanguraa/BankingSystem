using BankingSystem.Core.Models.Requests;

namespace BankingSystem.Core.Interfaces
{
    public interface IOperatorRepository
    {
        Task<bool> AddOperator(CreateOperatorRequest request);
        Task<bool> OperatorExists(string username);
    }
}