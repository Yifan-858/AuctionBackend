using AuctionBackend.Data.DTO;
using AuctionBackend.Data.Entities;
using AuctionBackend.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuctionBackend.Data.Repos
{
    public class UserRepo:IUserRepo
    {
        private readonly AppDbContext _appDbContext;
        public UserRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

         public async Task<User> CreateUser(string userName, string email, string passwordHash)
        {
            var user = new User(userName,email,passwordHash);
            _appDbContext.Users.Add(user);
            await _appDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _appDbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

    }
}
