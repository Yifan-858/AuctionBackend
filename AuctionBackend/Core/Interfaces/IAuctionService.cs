using AuctionBackend.Data.DTO;

namespace AuctionBackend.Core.Interfaces
{
    public interface IAuctionService
    {
        Task<AuctionCreateDto> CreateAuction(string title, string description, decimal startPrice, DateTime startDateUtc, DateTime endDateUtc, int userId);
        Task<List<AuctionDto>> GetAllAuctions();

        Task<List<AuctionDto>> GetAuctionsByUser(int userId);

        Task<AuctionDto> GetAuctionById(int auctionId);
        
        Task<List<AuctionDto>> GetAuctionsByTitle(string title);
       
        Task<AuctionDto> UpdateAuction(int auctionId, string? title, string? description, decimal? startPrice);
 
        Task DeleteAuction(int auctionId, int userId);
    }
}
