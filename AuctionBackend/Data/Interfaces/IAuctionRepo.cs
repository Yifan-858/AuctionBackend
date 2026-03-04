using AuctionBackend.Data.Entities;

namespace AuctionBackend.Data.Interfaces
{
    public interface IAuctionRepo
    {
        Task<Auction> CreateAuction(string title, string description, decimal startPrice, DateTime startDateUtc, DateTime endDateUtc,int userId);
        Task<List<Auction>> GetAllAuctions();
        Task<List<Auction>> GetAuctionsByUser(int userId);

        Task<Auction> GetAuctionById(int auctionId);

        Task<List<Auction>> GetAuctionsByTitle(string title);

        Task<Auction> UpdateAuction(int auctionId, string? title, string? description, decimal? startPrice);

        Task<bool> DeleteAuction(int auctionId);
    }
}
