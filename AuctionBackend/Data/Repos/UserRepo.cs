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

        public async Task<User?> GetUserById(int id)
        {
            return await _appDbContext.Users.SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> UpdateUserPassword(int id, string passwordHash)
        {
            var user = await _appDbContext.Users.SingleOrDefaultAsync(u => u.Id == id);

            if(user == null)
            {
                throw new KeyNotFoundException($"User with id{id} not found");
            }

            if (!string.IsNullOrEmpty(passwordHash))
            {
                user.PasswordHash = passwordHash;
            }
            await _appDbContext.SaveChangesAsync();

            return user;
        }

        public async Task DeleteUser(User user)
        {
            var auctions = _appDbContext.Auctions
                .Where(p => p.UserId == user.Id);

            var bids = _appDbContext.Bids
                .Where(c => c.UserId == user.Id);

            var auctionIds = auctions.Select(p => p.Id);

            var bidsOnAuctions = _appDbContext.Bids
                .Where(b => auctionIds.Contains(b.AuctionId));

            _appDbContext.Bids.RemoveRange(bidsOnAuctions);
            _appDbContext.Bids.RemoveRange(bids);
            _appDbContext.Auctions.RemoveRange(auctions);

            _appDbContext.Users.Remove(user);

            await _appDbContext.SaveChangesAsync();
        }

    }
}
