using AuctionBackend.Data.DTO;

namespace AuctionBackend.Core.Interfaces
{
    public interface IBidService
    {
        Task<BidDto> AddBid(decimal bidAmount, int userId, int auctionId);

        Task<List<BidDto>> GetAllBids();

        Task<List<BidDto>> GetBidByUser(int userId);

        Task<List<BidDto>> GetBidByAuction(int auctionId);

        Task<bool> DeleteBid(int bidId);
    }
}
