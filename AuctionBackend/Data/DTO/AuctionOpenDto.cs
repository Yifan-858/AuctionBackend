using AuctionBackend.Data.Entities;

namespace AuctionBackend.Data.DTO
{
    public class AuctionOpenDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal StartPrice { get; set; } 

        public DateTime StartDateUtc { get; set; }
        public DateTime EndDateUtc { get; set; }

        public string UserName { get; set; }

        public decimal? HighestBid { get; set; }
        public List<Bid>? Bids { get; set; } = new();

        public bool IsOpen { get; set; }
        public bool IsOwner { get; set; }
        public bool CanBid { get; set; }
    }
}
