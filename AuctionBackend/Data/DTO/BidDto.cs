namespace AuctionBackend.Data.DTO
{
    public class BidDto
    {
        public int Id { get; set; }
        public decimal BidAmount { get; set; } 
        public DateTime CreatedAtUtc { get; set; }
        public string UserName { get; set; }
        public int AuctionId { get; set; }
    }
}
