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
            return await _appDbContext.Auctions
                        .Include(a=>a.User)
                        .Include(a=> a.Bids)
                        .SingleOrDefaultAsync(a=> a.Id == auctionId);
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
            

            if(!string.IsNullOrEmpty(title))
            {
                auction.Title = title;
            }

            if (!string.IsNullOrEmpty(description))
            {
                auction.Description = description;
            }

            if (startPrice.HasValue)
            {
                if(auction.Bids != null && auction.Bids.Any(b => !b.IsDeleted))
                {
                    throw new Exception($"Cannot update the start price, auction id{auctionId} has bids already.");
                }

                if (startPrice.Value <= 0)
                {
                    throw new Exception("Start price must be greater than 0.");
                }
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
