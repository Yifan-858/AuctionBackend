using AuctionBackend.Data.Entities;

namespace AuctionBackend.Data.DTO
{
    public class AuctionCloseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal StartPrice { get; set; } 
        public decimal? SoldPrice { get; set; }
        public DateTime StartDateUtc { get; set; }
        public DateTime EndDateUtc { get; set; }

        public string UserName { get; set; }

        public bool IsOpen { get; set; } 
    }
}
