using AuctionBackend.Data.DTO;
using AuctionBackend.Data.Entities;
using System.Linq;
using AutoMapper;

namespace AuctionBackend.Data.Profiles
{
    public class AuctionProfile:Profile
    {
       public AuctionProfile()
        {
            CreateMap<Auction, AuctionCreateDto>();

            CreateMap<Auction, AuctionDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(org => org.User.UserName))
                .ForMember(dest => dest.HighestBid, opt => opt.MapFrom(src => src.Bids != null && src.Bids.Any(b => !b.IsDeleted)
                    ? src.Bids.Where(b => !b.IsDeleted).Max(b => b.BidAmount)
                    : (decimal?)null))
                .ForMember(dest => dest.SoldPrice, opt => opt.MapFrom(src =>
                    (src.IsActive && src.StartDateUtc <= DateTime.UtcNow && src.EndDateUtc > DateTime.UtcNow)
                        ? (decimal?)null
                        : (src.Bids != null && src.Bids.Any(b => !b.IsDeleted)
                            ? src.Bids.Where(b => !b.IsDeleted).Max(b => (decimal?)b.BidAmount)
                            : (decimal?)null)))
                .ForMember(dest => dest.IsOpen, opt => opt.MapFrom(src => src.IsActive && 
                                                                          src.EndDateUtc > DateTime.UtcNow))
                .ForMember(dest => dest.BidCount, opt => opt.MapFrom(src => src.Bids != null 
                                                           ? src.Bids.Count(b => !b.IsDeleted) 
                                                           : 0))
                .ForMember(dest => dest.Bids, opt => opt.MapFrom(src => src.Bids != null 
                                                ? src.Bids.Where(b => !b.IsDeleted)
                                                : new List<Bid>()));
        }
    }
}
