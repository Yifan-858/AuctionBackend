using AuctionBackend.Data.DTO;

namespace AuctionBackend.Core.Interfaces
{
    public interface IAuctionService
    {
        Task<AuctionCreateDto> CreateAuction(string title, string description, decimal startPrice, DateTime startDateUtc, DateTime endDateUtc, int userId);
    }
}
