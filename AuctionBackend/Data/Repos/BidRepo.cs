using AuctionBackend.Data.Entities;
using AuctionBackend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuctionBackend.Data.Repos
{
    public class BidRepo:IBidRepo
    {
        private readonly AppDbContext _appDbContext;
        public BidRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Bid> AddBid(decimal bidAmount, DateTime createdAtUtc, int userId, int auctionId)
        {
            var bid = new Bid(bidAmount,createdAtUtc, userId, auctionId);
            _appDbContext.Bids.Add(bid);
            await _appDbContext.SaveChangesAsync();

            return bid;
        }

        public async Task<List<Bid>> GetAllBids()
        {
            return await _appDbContext.Bids
                            .Include(b => b.User)
                            .Include(b => b.Auction)
                            .ToListAsync();
        }

        public async Task<Bid> GetSingleBid(int bidId)
        {
            return await _appDbContext.Bids.FindAsync(bidId);
        }

        public async Task<List<Bid>> GetBidsByUser(int userId)
        {
            return await _appDbContext.Bids.Where(b => b.UserId == userId).ToListAsync();
           
        }

        public async Task<List<Bid>> GetBidsByAuction(int postId)
        { 
            return await _appDbContext.Bids.Where(b=> b.AuctionId == postId).ToListAsync();
        }

        public async Task<bool> DeleteBid(int bidId)
        {
            var bid = await _appDbContext.Bids.FindAsync(bidId);
            if(bid == null)
            {
                return false; 
            }

            _appDbContext.Bids.Remove(bid);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

    }
}
