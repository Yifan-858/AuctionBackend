namespace AuctionBackend.Data.DTO
{
    public class BidCreateDto
    {
        public decimal BidAmount { get; set;  }
        public int AuctionId { get; set; }
    }
}
