using AuctionBackend.Data.Entities;

namespace AuctionBackend.Data.Interfaces
{
    public interface IUserRepo
    {
        Task<User> CreateUser(string userName, string email, string passwordHash);
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserById(int id);
        Task<User> UpdateUserPassword(int id, string passwordHash);
        Task DeleteUser(User user);
    }
}
