namespace BankingSystem.Core.Features.Users
{
    public interface IUserRepository
    {
        Task<bool> UserByPersonalIdExist(string personalId);
        Task<bool> UserExistsAsync(int userId);
    }
}
