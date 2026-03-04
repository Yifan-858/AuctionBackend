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
    }
}
