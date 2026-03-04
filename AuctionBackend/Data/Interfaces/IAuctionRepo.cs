using AuctionBackend.Data.Entities;

namespace AuctionBackend.Data.Interfaces
{
    public interface IAuctionRepo
    {
        Task<Auction> CreateAuction(string title, string description, decimal startPrice, DateTime startDateUtc, DateTime endDateUtc,int userId);
    }
}
