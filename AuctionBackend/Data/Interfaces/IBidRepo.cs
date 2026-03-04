using AuctionBackend.Data.Entities;

namespace AuctionBackend.Data.Interfaces
{
    public interface IBidRepo
    {

        Task<Bid> AddBid(decimal bidAmount, DateTime createdAtUtc, int userId, int auctionId);

        Task<List<Bid>> GetAllBids();

        Task<Bid> GetSingleBid(int bidId);

        Task<List<Bid>> GetBidsByUser(int userId);

        Task<List<Bid>> GetBidsByAuction(int postId);

        Task<bool> DeleteBid(int bidId);
    }
}
