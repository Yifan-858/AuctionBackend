using AuctionBackend.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace AuctionBackend.Data.DTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
