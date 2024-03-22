namespace BankingSystem.Core.Features.Users
{
    public interface IUserRepository
    {
        Task<bool> UserByPersonalIdExist(string personalId);
    }
}
