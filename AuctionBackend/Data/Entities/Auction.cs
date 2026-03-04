using System.ComponentModel.DataAnnotations;

namespace AuctionBackend.Data.Entities
{
    public class Auction
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public decimal StartPrice { get; set; } = 1;
        public decimal? SoldPrice { get; set; }
        public DateTime StartDateUtc { get; set; }
        public DateTime EndDateUtc { get; set; }
        public bool IsActive { get; set; } = true;

        //Many to One
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
        //One to Many
        public List<Bid>? Bids { get; set; } = new();

        public Auction(string title, string description, decimal startPrice, DateTime startDateUtc, DateTime endDateUtc,int userId)
        {
            Title = title;
            Description = description;
            StartPrice = startPrice;
            StartDateUtc = startDateUtc;
            EndDateUtc = endDateUtc;
            UserId = userId;
        }
    }
}
