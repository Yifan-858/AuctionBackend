using AuctionBackend.Data.Entities;
using AuctionBackend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuctionBackend.Data.Repos
{
    public class AuctionRepo:IAuctionRepo
    {
        private readonly AppDbContext _appDbContext;
        public AuctionRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Auction> CreateAuction(string title, string description, decimal startPrice, DateTime startDateUtc, DateTime endDateUtc,int userId)
        {
            var auction = new Auction(title, description, startPrice, startDateUtc, endDateUtc, userId);
            _appDbContext.Auctions.Add(auction);
            await _appDbContext.SaveChangesAsync();

            await _appDbContext.Entry(auction)
                  .Reference(a => a.User)
                  .LoadAsync();

            return auction;
        }
        public async Task<List<Auction>> GetAllAuctions()
        {
            return await _appDbContext.Auctions
                .Include(a => a.User)
                .Include(a => a.Bids)
                .ToListAsync();
        }
        public async Task<List<Auction>> GetAuctionsByUser(int userId)
        {
            return await _appDbContext.Auctions.Where(a=> a.UserId == userId).ToListAsync();
        }

        public async Task<Auction> GetAuctionById(int auctionId)
        {
            return await _appDbContext.Auctions.SingleOrDefaultAsync(a=> a.Id == auctionId);
        }

        public async Task<List<Auction>> GetAuctionsByTitle(string title)
        { 
            title = title.Trim().ToLower();
            var auctions = await _appDbContext.Auctions
                .Where(a => a.Title != null && a.Title.ToLower().Contains(title))
                .Include(a=> a.Bids)
                .Include(a=> a.User)
                .ToListAsync();

            return auctions;
        }

         public async Task<Auction> UpdateAuction(int auctionId, string? title, string? description, decimal? startPrice)
        {
            var auction = await _appDbContext.Auctions.FindAsync(auctionId);//search for primary key

            if(auction == null)
            {
                throw new KeyNotFoundException($"Auction id{auctionId} not found");
            }
            else if(auction.Bids != null && auction.Bids.Count > 0)
            {
                throw new Exception($"Cannot delete auction id{auctionId}. It has bids already.");
            }

            if(!string.IsNullOrEmpty(title))
            {
                auction.Title = title;
            }

            if (!string.IsNullOrEmpty(description))
            {
                auction.Description = description;
            }

            if(startPrice > 0)
            {
                auction.StartPrice = (decimal)startPrice;
            }

            await _appDbContext.SaveChangesAsync();
            return auction;
        }

        public async Task<bool> DeleteAuction(int auctionId)
        {
            var auction = await _appDbContext.Auctions.FindAsync(auctionId);
            if(auction == null)
            {
                return false;
            }

            _appDbContext.Auctions.Remove(auction);
            await _appDbContext.SaveChangesAsync();

            return true;
        }
    }
}
