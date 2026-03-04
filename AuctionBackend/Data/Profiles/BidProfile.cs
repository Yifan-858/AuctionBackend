using AuctionBackend.Data.DTO;
using AuctionBackend.Data.Entities;
using AutoMapper;

namespace AuctionBackend.Data.Profiles
{
    public class BidProfile:Profile
    {
        public BidProfile()
        {
            CreateMap<Bid, BidDto>()
           .ForMember(dest => dest.UserName, opt => opt.MapFrom(org => org.User.UserName));
        }
       
    }
}
