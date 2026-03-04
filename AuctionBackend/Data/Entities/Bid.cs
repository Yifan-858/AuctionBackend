using System.ComponentModel.DataAnnotations;

namespace AuctionBackend.Data.Entities
{
    public class Bid
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public decimal BidAmount { get; set; } 
        public DateTime CreatedAtUtc { get; set; }
        public bool IsDeleted { get; set; } = false;

        //Many to one
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        //Many to one
        [Required]
        public int AuctionId { get; set; }
        public Auction Auction { get; set; }

        public Bid(decimal bidAmount, DateTime createdAtUtc, int userId, int auctionId)
        {
            BidAmount = bidAmount;
            CreatedAtUtc = createdAtUtc;
            UserId = userId;
            AuctionId = auctionId;
        }
    }
}
