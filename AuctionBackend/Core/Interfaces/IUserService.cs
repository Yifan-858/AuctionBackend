using AuctionBackend.Data.DTO;
using AuctionBackend.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AuctionBackend.Core.Interfaces
{
    public interface IUserService
    {
        Task<string> RegisterUser([FromBody] SignupDto signupDto);
        string GenerateToken(User user);
        Task<User> Login([FromBody] LoginDto login);
        Task<User> GetUserById(int userId);
        Task<User> UpdateUserPassword(int userId, string newPassword);
        Task DeleteUser(int userId);
    }
}
