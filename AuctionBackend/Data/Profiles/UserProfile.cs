using AuctionBackend.Data.DTO;
using AuctionBackend.Data.Entities;
using AutoMapper;

namespace AuctionBackend.Data.Profiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
