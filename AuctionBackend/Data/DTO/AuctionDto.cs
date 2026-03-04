using AuctionBackend.Data.Entities;

namespace AuctionBackend.Data.DTO
{
    public class AuctionDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal StartPrice { get; set; } 
        public DateTime StartDateUtc { get; set; }
        public DateTime EndDateUtc { get; set; }
        public decimal? SoldPrice { get; set; }
        public decimal? HighestBid { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }

        public int BidCount { get; set; }
        public List<Bid>? Bids { get; set; } = new();

        public bool IsActive { get; set; }
        public bool IsOpen { get; set; }
    }
}
