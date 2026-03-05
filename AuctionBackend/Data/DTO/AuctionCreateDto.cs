using AuctionBackend.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace AuctionBackend.Data.DTO
{
    public class AuctionCreateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal StartPrice { get; set; }
        public DateTime StartDateUtc { get; set; }
        public DateTime EndDateUtc { get; set; }
    }
}
